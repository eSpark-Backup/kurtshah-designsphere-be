using Microsoft.AspNetCore.Identity;
using System;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;

public class UserIdentity : IdentityUser
{
    public EUserType Type { get; set; }
    public DateTime CreationDate { get; set; }
}
