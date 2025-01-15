using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Common.Services;
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
    public string GetFullName()
    {
        return _IHttpContextAccessor.HttpContext.User?.FindFirst("fullname")?.Value;
    }
    public string GetCompanyName()
    {
        return _IHttpContextAccessor.HttpContext.User?.FindFirst("companyname")?.Value;
    }
    public string GetPhoneNumber()
    {
        return _IHttpContextAccessor.HttpContext.User?.FindFirst(JwtClaimTypes.PhoneNumber)?.Value;
    }
    public string GetUserEmail()
    {
        return _IHttpContextAccessor.HttpContext.User?.FindFirst(JwtClaimTypes.Email)?.Value;
    }
    public string GetDeviceId()
    {
        return _IHttpContextAccessor.HttpContext.User?.FindFirst("device_id")?.Value;
    }
}
