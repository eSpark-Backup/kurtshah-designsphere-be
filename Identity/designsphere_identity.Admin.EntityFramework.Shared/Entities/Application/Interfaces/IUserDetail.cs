namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

public interface IUserDeviceBase
{
    public string DeviceCode { get; set; }
}

public interface IUserDetailBase
{
    string AppHash { get; set; }
    string NotificationId { get; set; }
}
