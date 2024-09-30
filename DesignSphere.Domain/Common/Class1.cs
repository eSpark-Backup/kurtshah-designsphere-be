using DesignSphere.Domain.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignSphere.Domain.Common;

public interface IEntityBase
{
    DateTime CreationTime { get; set; }
    Guid CreatorId { get; set; }
    string? CreatorName { get; set; }
    Guid Id { get; set; }
    DateTime? LastmodificationTime { get; set; }
    Guid? LastmodifierId { get; set; }
    string? LastmodifierName { get; set; }
}

public class EntityBase : IEntityBase
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string? CreatorName { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid? LastmodifierId { get; set; }
    public string? LastmodifierName { get; set; }
    public DateTime? LastmodificationTime { get; set; }
}
