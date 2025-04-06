using EventPlanner.Application.Responses;
using EventPlanner.Application.Services.EventService.Dto;

namespace EventPlanner.Application.Interfaces;
public interface IEventService
{
    public Task<Response<GetEventsResponseDto>> GetEvents(Guid TenantId, GetEventsRequestDto request);
    public Task<Response<string>> CreateUpdate(Guid TenantId, CreateUpdateEventRequestDto request);
    public Task<Response<string>> Delete(DeleteEventRequestDto request);
}
