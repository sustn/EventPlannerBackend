using System.ComponentModel;

namespace EventPlanner.Domain.Common;
public interface ISoftDelete
{
    [DefaultValue(false)]
    bool isDeleted { get; set; }
}
