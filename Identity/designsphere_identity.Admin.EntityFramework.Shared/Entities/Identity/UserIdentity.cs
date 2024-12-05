using Microsoft.AspNetCore.Identity;
using System;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity; 
public interface IAppUserExtention
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string ProfilePictureUrl { get; set; }
    bool? NewsLetterSubscription { get; set; }
 
}
public class UserIdentity : IdentityUser, IAppUserExtention
{
    public EUserType Type { get; set; }
    public DateTime CreationDate { get; set; } 
    public string FirstName { get; set; } =string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ProfilePictureUrl { get; set; } = string.Empty;
    public bool? NewsLetterSubscription { get; set; }
  

}
