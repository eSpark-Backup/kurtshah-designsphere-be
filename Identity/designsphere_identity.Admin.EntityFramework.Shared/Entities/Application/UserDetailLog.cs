using System;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;

public class UserDetailLog : EntityBase, IUserDetailBase, IOTP, IName
{
    public Guid UserDetailId { get; set; }
    public string UserIdentityId { get; set; }
    public string UserName { get; set; }
    public byte OTPRetries { get; set; }
    public EOTPStatus OTPStatus { get; set; }
    public string Name { get; set; }
    public string AppHash { get; set; }
    public string NotificationId { get; set; }
    public Guid DeviceId { get; set; }


}
