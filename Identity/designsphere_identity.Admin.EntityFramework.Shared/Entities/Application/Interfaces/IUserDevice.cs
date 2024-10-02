
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

public interface IDevice
{
    string DeviceCode { get; set; }
    EDeviceStatus DeviceStatus { get; set; }
}
