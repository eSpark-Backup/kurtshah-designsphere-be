using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.STS.Identity.CustomGrants.Models;
using designsphere_identity.STS.Identity.CustomGrants.Services;

namespace designsphere_identity.STS.Identity.Controllers;

[ApiController]
[Route("[controller]")]
public class DevOtpController : ControllerBase
{
    private readonly IOTPService oTPService;
    private readonly IHostEnvironment hostEnvironment;
    private readonly UserManager<UserIdentity> userManager;

    public DevOtpController(IOTPService oTPService,
        IHostEnvironment hostEnvironment,
        UserManager<UserIdentity> userManager)
    {
        this.oTPService = oTPService;
        this.hostEnvironment = hostEnvironment;
        this.userManager = userManager;
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> ApiResponse()
    {
        return new ApiResponse($"Environment {hostEnvironment.EnvironmentName}", false);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> GenerateOTP([FromQuery] DevOtpDto req)
    {
        if (hostEnvironment.IsProduction())
            return new ApiResponse($"Environment {hostEnvironment.EnvironmentName}", false);

        var user = await userManager.Users.FirstOrDefaultAsync(_ => _.PhoneNumber == req.PhoneNumber);
        if (user == null)
        {
            return new ApiResponse(new System.Exception("Phonenumber Not Found"));
        }
        return new ApiResponse($"Otp for Development pupose only{req.PhoneNumber}", oTPService.GenerateOTP(user.UserName, req.AppHash, req.GrantType));
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
}
