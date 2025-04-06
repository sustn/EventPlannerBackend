using EventPlanner.Application.Interfaces;
using EventPlanner.Application.Responses;
using EventPlanner.Application.Services.EventService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.WebAPI.Controllers;
[ApiController]
[Route("event")]
public class EventController(IEventService eventService) : ControllerBase
{
    private IEventService _eventService = eventService;

    [HttpGet]
    [Route("")]
    public async Task<Response<GetEventsResponseDto>> Index([FromQuery] GetEventsRequestDto request)
            => await _eventService.GetEvents(Guid.Parse(HttpContext.Items["TenantId"]?.ToString()!), request);

    [HttpPost]
    [Route("createUpdate")]
    public async Task<Response<string>> CreateUpdate([FromBody] CreateUpdateEventRequestDto request)
            => await _eventService.CreateUpdate(Guid.Parse(HttpContext.Items["TenantId"]?.ToString()!), request);

    [HttpPost]
    [Route("delete")]
    public async Task<Response<string>> DeleteEvent([FromBody] DeleteEventRequestDto request)
            => await _eventService.Delete(request);
}
