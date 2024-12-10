using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.STS.Identity.CustomGrants.Models;
using designsphere_identity.STS.Identity.CustomGrants.Services;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.STS.Identity.CustomGrants.Helpers;
using designsphere_identity.STS.Identity.CustomGrants;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

namespace designsphere_identity.STS.Identity.Controllers;

[ApiController]
[Route("[controller]")]
public class DevOtpController : ControllerBase
{
    private readonly IOTPService oTPService;
    private readonly IHostEnvironment hostEnvironment;
    private readonly UserManager<UserIdentity> userManager;
    private readonly RoleManager<UserIdentityRole> _roleManager;

    public DevOtpController(IOTPService oTPService,
        IHostEnvironment hostEnvironment,
        UserManager<UserIdentity> userManager,
        RoleManager<UserIdentityRole> roleManager)
    {
        this.oTPService = oTPService;
        this.hostEnvironment = hostEnvironment;
        this.userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> ApiResponse()
    {
        return new ApiResponse($"Environment {hostEnvironment.EnvironmentName}", false);
    }

    [HttpGet("GetOtp")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> GetOTP([FromQuery] DevOtpDto req)
    {
        if (hostEnvironment.IsProduction())
            return new ApiResponse($"Environment {hostEnvironment.EnvironmentName}", false);

        var user = await userManager.Users.FirstOrDefaultAsync(_ => _.Email == req.Email);
        if (user == null)
        {
            return new ApiResponse(new System.Exception("Email Not Found"));
        }
        return new ApiResponse($"Otp for Development pupose only{req.Email}", oTPService.GenerateOTP(user.UserName, req.AppHash, req.GrantType));
    }

    [HttpPost("[action]")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> DeleteUser([FromQuery] DelUserDto req)
    {
        if (hostEnvironment.IsProduction())
            return new ApiResponse($"Environment {hostEnvironment.EnvironmentName}", false);

        var user = await userManager.Users.FirstOrDefaultAsync(_ => _.UserName == req.UserName);
        if (user == null)
        {
            return new ApiResponse(new System.Exception("User Not Found"));
        }
        await userManager.DeleteAsync(user);
        return new ApiResponse($"User {req.UserName} deleted", true);
    }
}

public class DelUserDto
{
    public string UserName { get; set; }
}

public class DevOtpDto
{
    public string PhoneNumber { get; set; }
    public string AppHash { get; set; }
    public string GrantType { get; set; }
    public string Email { get; set; }

}
