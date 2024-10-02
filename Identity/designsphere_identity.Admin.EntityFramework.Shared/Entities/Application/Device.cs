using System.Collections.Generic;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;

public class Device : EntityBase, IDevice
{
    public string DeviceCode { get; set; }
    public EDeviceStatus DeviceStatus { get; set; }
    public virtual ICollection<UserDetail> UserDetails { get; set; }
}
