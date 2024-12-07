using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;
using System;

namespace designsphere_identity.STS.Identity.CustomGrants.Models;

public class EmailGrantRequest : IOTPToken ,IEmailGrantRequest
{
    public EPhoneOtp? OtpType { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? Otp { get; set; }
    public string? ProtectToken { get; set; }
    public string? Token { get; set; }

    public string DeviceCode { get; set; }
    public Boolean ForgotPassword { get; set; }


}