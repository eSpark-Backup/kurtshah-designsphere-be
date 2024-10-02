using System;
using System.ComponentModel.DataAnnotations.Schema;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
public class UserDetail : EntityBase, IUserDetailBase, IOTP, IName
{
    [ForeignKey("UserIdentity")]
    public string UserIdentityId { get; set; }

    public string UserName { get; set; }
    public string NotificationId { get; set; }
    public string AppHash { get; set; }
    public byte OTPRetries { get; set; }
    public EOTPStatus OTPStatus { get; set; }

    [ForeignKey("Device")]
    public Guid DeviceId { get; set; }

    public string Name { get; set; }
}
