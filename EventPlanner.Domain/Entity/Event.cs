using EventPlanner.Domain.Common;

namespace EventPlanner.Domain.Entity;
public class Event : IEntity, IHasCreationTime, IHasModificationTime, IActive, ISoftDelete
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public bool isActive { get; set; }
    public bool isDeleted { get; set; }

}
