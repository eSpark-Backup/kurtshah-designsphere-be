using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;

namespace designsphere_identity.STS.Identity.CustomGrants.Services;

public interface IUserService
{
    string GetUserIdentity();

    string GetClientId();

    string GetUserName();

    string GetPhoneNumber();

    string GetDeviceId();
}

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _IHttpContextAccessor;

    public UserService(IHttpContextAccessor context)
    {
        _IHttpContextAccessor = context ?? throw new ArgumentNullException(nameof(context));
    }

    public string GetUserIdentity()
    {
        return _IHttpContextAccessor.HttpContext.User.FindFirst(JwtClaimTypes.Subject).Value;
    }

    public string GetClientId()
    {
        return _IHttpContextAccessor.HttpContext.User.FindFirst(JwtClaimTypes.ClientId).Value;
    }

    public string GetUserName()
    {
        return _IHttpContextAccessor.HttpContext.User.Identity.Name;
    }

    public string GetPhoneNumber()
    {
        return _IHttpContextAccessor.HttpContext.User?.FindFirst(JwtClaimTypes.PhoneNumber)?.Value;
    }

    public string GetDeviceId()
    {
        return _IHttpContextAccessor.HttpContext.User?.FindFirst("device_id")?.Value;
    }
}
