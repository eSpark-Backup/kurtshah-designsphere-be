using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

namespace designsphere_identity.STS.Identity.CustomGrants.Models;

public class OauthResult
{
    public bool IsIdTokenValid { get; set; } = true;
    public bool IsPhoneNumberValid { get; set; }
    public string ProtectToken { get; set; } = string.Empty;

    public OauthResult(bool isPhoneNumberValid, string protectToken)
    {
        IsPhoneNumberValid = isPhoneNumberValid;
        ProtectToken = protectToken;
    }

    public OauthResult(bool isIdTokenValid)
    {
        IsIdTokenValid = isIdTokenValid;
    }
}

public class UserInfoDev : UserInfo 
{
    public string ProtectToken { get; set; } = string.Empty;
}
public class UserInfo : IUserDetailBase, IName
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string OauthType { get; set; }
    public string Name { get; set; }
    public string AppHash { get; set; }
    public string NotificationId { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool Is_Email_Authenticated { get; set; }
    public bool Is_Google_Authenticated { get; set; }
    public bool UserProfile { get; set; }

}
