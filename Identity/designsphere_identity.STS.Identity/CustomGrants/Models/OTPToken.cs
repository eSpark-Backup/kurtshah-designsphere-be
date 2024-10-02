using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

namespace designsphere_identity.STS.Identity.CustomGrants.Models;

public class OTPResponse : IOTPTokenResponse
{
    public string ProtectToken { get; set; }

    public OTPResponse(string protectToken)
    {
        ProtectToken = protectToken;
    }
}

public class OTPToken : IOTPToken
{
    public string Otp { get; set; }
    public string ProtectToken { get; set; }
}
