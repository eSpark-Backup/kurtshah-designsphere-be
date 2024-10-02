using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

namespace designsphere_identity.STS.Identity.CustomGrants.Models;

public class BackendGrantRequest
{
    public string IdentityId { get; set; }
}

public class PhoneGrantRequest : IPhoneGrantRequest, IOTPToken, IGoogleRequest
{
    public EPhoneOtp? OtpType { get; set; }
    public string AppHash { get; set; }
    public string NotificationId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Otp { get; set; }
    public string? ProtectToken { get; set; }
    public string DeviceCode { get; set; }
    public string? IdToken { get; set; }
}
