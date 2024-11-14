using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;


namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;
public interface IEmailGrantRequest : IUserDeviceBase
{
    EPhoneOtp? OtpType { get; set; }

}
