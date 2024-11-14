using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignSphere.Domain.Common;

public interface IEntityModifier 
{
    DateTime? LastmodificationTime { get; set; }
    string? LastmodifierId { get; set; }
    string? LastmodifierName { get; set; }
}
public interface IEntityCreator
{
    DateTime CreationTime { get; set; }
    string? CreatorId { get; set; }
    string? CreatorName { get; set; }
}
public interface IEntityId 
{
    Ulid Id { get; set; }

}
public interface IEntityBase :  IEntityCreator, IEntityModifier
{

    
}

public class EntityIdentity : IEntityId
{
    public Ulid Id { get; set; }
}
public class EntityBase : EntityIdentity, IEntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string? CreatorId { get; set; }
    public string? CreatorName { get; set; }
    public DateTime CreationTime { get; set; }
    public string? LastmodifierId { get; set; }
    public string? LastmodifierName { get; set; }
    public DateTime? LastmodificationTime { get; set; }
}
