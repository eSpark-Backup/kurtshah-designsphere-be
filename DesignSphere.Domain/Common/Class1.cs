using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignSphere.Domain.Common;

public interface IEntityBase
{
    DateTime CreationTime { get; set; }
    Ulid CreatorId { get; set; }
    string? CreatorName { get; set; }
    Ulid Id { get; set; }
    DateTime? LastmodificationTime { get; set; }
    Ulid? LastmodifierId { get; set; }
    string? LastmodifierName { get; set; }
}

public class EntityBase : IEntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public Ulid CreatorId { get; set; }
    public string? CreatorName { get; set; }
    public DateTime CreationTime { get; set; }
    public Ulid? LastmodifierId { get; set; }
    public string? LastmodifierName { get; set; }
    public DateTime? LastmodificationTime { get; set; }
}
