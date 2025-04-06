using System.ComponentModel;

namespace EventPlanner.Domain.Common;
public interface IActive
{
    [DefaultValue(true)]
    bool isActive { get; set; }
}
