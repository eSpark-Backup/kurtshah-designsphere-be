using System;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;

public class EntityBase
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreationTime { get; set; }
    public string CreatorId { get; set; }
    public DateTime? LastmodificationTime { get; set; }
}
