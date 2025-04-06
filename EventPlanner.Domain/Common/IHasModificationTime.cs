namespace EventPlanner.Domain.Common;
public interface IHasModificationTime
{
    DateTime LastModifiedDate { get; set; }
}
