namespace BuildingBlocks.Common.Services;

public interface IUserService
{
    string GetUserIdentity();

    string GetClientId();

    string GetUserName();

    string GetPhoneNumber();

    string GetDeviceId();
}
