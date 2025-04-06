namespace EventPlanner.Application.Services.EventService.Dto;
public class InviteDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class EventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<InviteDto> Invites { get; set; } = new();
}

public class GetEventsResponseDto
{
    public int TotalRecords { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public List<EventDto> Data { get; set; } = new();
}
