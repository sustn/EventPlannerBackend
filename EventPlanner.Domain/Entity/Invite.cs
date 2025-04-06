using EventPlanner.Domain.Common;

namespace EventPlanner.Domain.Entity;
public class Invite: IEntity, IHasCreationTime, IHasModificationTime, IActive, ISoftDelete
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public bool isActive { get; set; }
    public bool isDeleted { get; set; }
}
