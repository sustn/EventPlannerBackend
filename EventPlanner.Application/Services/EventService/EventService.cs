using AutoMapper;
using EventPlanner.Application.Interfaces;
using EventPlanner.Application.Responses;
using EventPlanner.Application.Services.EventService.Dto;
using EventPlanner.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Application.Services.EventService;
public class EventService(IRepository<Invite> inviteRepository, IRepository<Event> eventRepository, IEmailService emailService, IMapper mapper) : IEventService
{
    private IRepository<Invite> _inviteRepository = inviteRepository;
    private IRepository<Event> _eventRepository = eventRepository;
    private IEmailService _emailService = emailService;
    private readonly IMapper _mapper = mapper;
    public async Task<Response<string>> CreateUpdate(Guid TenantId, CreateUpdateEventRequestDto request)
    {
        if (request == null)
            return new Response<string> { Message = "Invalid request", Success = false };

        return request.Id == Guid.Empty
            ? await CreateEventAsync(TenantId, request)
            : await UpdateEventAsync(request);
    }

    private async Task<Response<string>> CreateEventAsync(Guid TenantId, CreateUpdateEventRequestDto request)
    {
        var newEvent = _mapper.Map<Event>(request);
        newEvent.TenantId = TenantId;
        var eventDetail = await _eventRepository.AddAndGetId(newEvent);
        foreach (var inv in request.Invites)
        {
            var invitation = _mapper.Map<Invite>(inv);
            invitation.EventId = eventDetail.Id;
            await _inviteRepository.AddAndGetId(invitation);
            await _emailService.sendInvitaionEmail(invitation.Email, eventDetail);
        }

        return new Response<string>
        {
            Message = "Event created successfully",
            Result = eventDetail.Id.ToString(),
            Success = true
        };
    }

    private async Task<Response<string>> UpdateEventAsync(CreateUpdateEventRequestDto request)
    {
        var existingEvent = await _eventRepository.TableNoTracking
            .FirstOrDefaultAsync(e => e.Id == request.Id);

        if (existingEvent == null)
        {
            return new Response<string>
            {
                Message = "Event not found",
                Success = false
            };
        }
        existingEvent.Name = request.Name;
        existingEvent.Venue = request.Venue;
        existingEvent.StartTime = request.StartTime;
        existingEvent.EndTime = request.EndTime;
        await _eventRepository.Update(existingEvent);

        var existingInvites = await _inviteRepository.TableNoTracking
            .Where(i => i.EventId == existingEvent.Id)
            .ToListAsync();

        var existingInviteDict = existingInvites.ToDictionary(i => i.Id, i => i);
        var requestInviteIds = request.Invites
            .Where(inv => inv.Id != Guid.Empty)
            .Select(inv => inv.Id)
            .ToHashSet();
        var invitesToDelete = existingInvites
            .Where(i => !requestInviteIds.Contains(i.Id))
            .ToList();

        var invitesToUpdate = new List<Invite>();
        var invitesToAdd = new List<Invite>();

        foreach (var reqInvite in request.Invites)
        {
            if (reqInvite.Id != Guid.Empty && existingInviteDict.TryGetValue(reqInvite.Id, out var existingInvite))
            {
                _mapper.Map(reqInvite, existingInvite);
                invitesToUpdate.Add(existingInvite);
            }
            else
            {
                var newInvite = _mapper.Map<Invite>(reqInvite);
                newInvite.EventId = existingEvent.Id;
                invitesToAdd.Add(newInvite);
            }
        }
        if (invitesToDelete.Any())
        {
            await _inviteRepository.Delete(invitesToDelete);
        }

        if (invitesToUpdate.Any())
        {
            await _inviteRepository.Update(invitesToUpdate);
        }

        if (invitesToAdd.Any())
        {
            await _inviteRepository.Add(invitesToAdd);
            foreach(var invite in invitesToAdd)
            {
                await _emailService.sendInvitaionEmail(invite.Email, existingEvent);
            }
        }

        return new Response<string>
        {
            Message = "Event updated successfully",
            Result = existingEvent.Id.ToString(),
            Success = true
        };
    }

    public async Task<Response<string>> Delete(DeleteEventRequestDto request)
    {
        var existingEvent = await _eventRepository.TableNoTracking
            .FirstOrDefaultAsync(e => e.Id == request.Id);

        if (existingEvent == null)
        {
            return new Response<string>
            {
                Message = "Event not found",
                Success = false
            };
        }

        await this._eventRepository.Delete(existingEvent);

        return new Response<string>
        {
            Message = "Event deleted successfully",
            Result = existingEvent.Id.ToString(),
            Success = true
        };
    }

    public async Task<Response<GetEventsResponseDto>> GetEvents(Guid TenantId, GetEventsRequestDto request)
    {
        if (request.PageNumber < 1 || request.PageSize < 1)
        {
            throw new ArgumentException("Invalid pagination request.");
        }

        var totalRecords = await _eventRepository.TableNoTracking
            .Where(e => 
            e.TenantId == TenantId)
            .CountAsync();

        var events = await _eventRepository.TableNoTracking
            .Where(e => e.TenantId == TenantId)
            .OrderBy(e => e.CreatedDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var eventIds = events.Select(e => e.Id).ToList();

        var invites = await _inviteRepository.TableNoTracking
            .Where(i => eventIds.Contains(i.EventId) && !i.isDeleted)
            .ToListAsync();

        var eventDtos = events.Select(e => new EventDto
        {
            Id = e.Id,
            Name = e.Name,
            Venue = e.Venue,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            CreatedDate = e.CreatedDate,
            Invites = invites
                .Where(i => i.EventId == e.Id)
                .Select(i => new InviteDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Email = i.Email
                }).ToList()
        }).ToList();

        var response = new GetEventsResponseDto
        {
            TotalRecords = totalRecords,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Data = eventDtos
        };
        return new Response<GetEventsResponseDto> { Message = "Events fetched succesfully", Errors = [], Result = response };
    }
}
