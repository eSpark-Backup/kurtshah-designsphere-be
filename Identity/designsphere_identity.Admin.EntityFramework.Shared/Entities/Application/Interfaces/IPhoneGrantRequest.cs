using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

public interface IPhoneGrantRequest : IUserDeviceBase
{
    EPhoneOtp? OtpType { get; set; }
}
