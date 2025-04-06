namespace EventPlanner.Application.Services.EventService.Dto;
public class CreateUpdateEventRequestDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Venue { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<CreateUpdateRequestInvites> Invites { get; set; } = new();

}

public class CreateUpdateRequestInvites
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}
