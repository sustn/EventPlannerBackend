using EventPlanner.Domain.Common;

namespace EventPlanner.Domain.Entity;
public class Email : IEntity, IHasCreationTime, IHasModificationTime, IActive, ISoftDelete
{
    public Guid Id { get; set; }
    public string To { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public bool isActive { get; set; }
    public bool isDeleted { get; set; }
}
