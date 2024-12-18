using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.STS.Identity.CustomGrants.Models;

namespace designsphere_identity.STS.Identity.CustomGrants.Helpers;

public static class GrantHelper
{
    public static UserInfo GetUserinfo(UserIdentity user, UserDetail userdetailCheck)
    {
        return new UserInfo()
        {
            Id = user.Id,
            PhoneNumber = user.PhoneNumber,
            OauthType = user.Type.ToString(),
            UserName = userdetailCheck.Name,//user.UserName,
            AppHash = userdetailCheck.AppHash,
            Name = userdetailCheck.Name,
            NotificationId = userdetailCheck.NotificationId,
            TwoFactorEnabled = user.TwoFactorEnabled,
            Is_Email_Authenticated = userdetailCheck.IsEmailMFA,
            Is_Google_Authenticated = userdetailCheck.IsGoogleMFA,
            UserProfile = !string.IsNullOrEmpty(userdetailCheck.CompanyName)? true : false
        };
    }

    public static Dictionary<string, object> ClassToDict(ApiResponse response)
    {
        return response.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          .ToDictionary(prop => prop.Name, prop => prop.GetValue(response, null));
    }
}
