using AutoMapper;
using EventPlanner.Application.Interfaces;
using EventPlanner.Application.Services.EventService;
using EventPlanner.Application.Services.EventService.Dto;
using EventPlanner.Domain.Entity;
using Moq;

namespace EventPlanner.Test;
public class EventServiceTests
{
    private readonly Mock<IRepository<Invite>> _inviteRepoMock;
    private readonly Mock<IRepository<Event>> _eventRepoMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly IMapper _mapper;
    private readonly EventService _eventService;

    public EventServiceTests()
    {
        _inviteRepoMock = new Mock<IRepository<Invite>>();
        _eventRepoMock = new Mock<IRepository<Event>>();
        _emailServiceMock = new Mock<IEmailService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateUpdateEventRequestDto, Event>();
            cfg.CreateMap<CreateUpdateRequestInvites, Invite>();
        });
        _mapper = config.CreateMapper();

        _eventService = new EventService(
            _inviteRepoMock.Object,
            _eventRepoMock.Object,
            _emailServiceMock.Object,
            _mapper);
    }

    [Fact]
    public async Task CreateUpdate_CreateEvent_ReturnsSuccessResponse()
    {
        var tenantId = Guid.NewGuid();
        var createRequest = new CreateUpdateEventRequestDto
        {
            Id = Guid.Empty,
            Name = "Test Event",
            Venue = "Test Venue",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(1),
            Invites = new List<CreateUpdateRequestInvites>
            {
                new CreateUpdateRequestInvites { Id = Guid.Empty, Email = "test@example.com", Name = "Test Invite" }
            }
        };

        var newEvent = _mapper.Map<Event>(createRequest);
        newEvent.TenantId = tenantId;
        newEvent.Id = Guid.NewGuid();

        _eventRepoMock
            .Setup(r => r.AddAndGetId(It.IsAny<Event>()))
            .ReturnsAsync(newEvent);

        _inviteRepoMock
            .Setup(r => r.AddAndGetId(It.IsAny<Invite>()))
            .ReturnsAsync(new Invite { Id = Guid.NewGuid() });

        var result = await _eventService.CreateUpdate(tenantId, createRequest);

        Assert.True(result.Success);
        Assert.Equal("Event created successfully", result.Message);
        Assert.Equal(newEvent.Id.ToString(), result.Result);
        _emailServiceMock.Verify(es => es.sendInvitaionEmail("test@example.com", newEvent), Times.Once);
    }
}
