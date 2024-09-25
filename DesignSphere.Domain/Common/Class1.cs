using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignSphere.Domain.Common;
public class EntityBase
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string? CreatorName { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid? LastmodifierId { get; set; }
    public string? LastmodifierName { get; set; }
    public DateTime? LastmodificationTime { get; set; }
}
