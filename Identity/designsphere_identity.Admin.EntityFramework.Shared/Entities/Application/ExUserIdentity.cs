using System;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;

public class ExUserIdentity : UserIdentity
{
    public string Remarks { get; set; }
    public DateTime CreationDate { get; set; }
}
