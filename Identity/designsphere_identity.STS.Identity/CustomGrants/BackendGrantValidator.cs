using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.STS.Identity.CustomGrants.Helpers;
using designsphere_identity.STS.Identity.CustomGrants.Models;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using designsphere_identity.STS.Identity.CustomGrants.Services;

namespace designsphere_identity.STS.Identity.CustomGrants;

public class BackendGrantValidator : IExtensionGrantValidator

{
    private readonly UserManager<UserIdentity> userManager;
    private readonly RoleManager<UserIdentityRole> roleManager;
    private readonly IUserDetailRepository userDetailRepository;
    private readonly IDeviceRepository userDeviceRepository;
    private readonly IOTPService oTPService;
    private readonly IEventService events;
    private readonly IServiceProvider serviceProvider;
    private readonly ISmsService smsService;

    public BackendGrantValidator(
        UserManager<UserIdentity> userManager,
        RoleManager<UserIdentityRole> roleManager,
        IUserDetailRepository userDetailRepository,
        IDeviceRepository userDeviceRepository,
        IOTPService oTPService,
        IEventService events,
        IServiceProvider serviceProvider,
        ISmsService smsService
        )
    {
        this.userDetailRepository = userDetailRepository;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.userDeviceRepository = userDeviceRepository;
        this.oTPService = oTPService;
        this.events = events;
        this.serviceProvider = serviceProvider;
        this.smsService = smsService;
    }

    public string GrantType => "backend";
    private BackendGrantRequest request = new();
    private Payload payload;

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        try
        {
            ApiResponse response;
            request.IdentityId = context.Request.Raw.Get("identityid");

            if (string.IsNullOrEmpty(request.IdentityId))
            {
                response = new("Parameters missing");
                CustomResult(context, response);
                return;
            }

            // check username
            var user = await userManager.Users.FirstOrDefaultAsync(_ => _.Id == request.IdentityId);
            if (user == null)
            {
                response = new($"IdentityId not found {request.IdentityId}");
                CustomResult(context, response);
                return;
            }

            // Check User Status
            if (user.LockoutEnabled && user.LockoutEnd >= DateTime.Now)
            {
                response = new($"IdentityId locked until {user.LockoutEnd}");
                CustomResult(context, response);
                return;
            }
            context.Result = new GrantValidationResult(user.Id, authenticationMethod: GrantType);
            await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, false));
            return;
        }
        catch (Exception ex)
        {
            ApiResponse response = new(ex);
            CustomResult(context, response);
        }
    }

    // Convert an object to a byte array
    private async void CustomResult(ExtensionGrantValidationContext context, ApiResponse response)
    {
        Dictionary<string, object> errorresult = GrantHelper.ClassToDict(response);
        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, customResponse: errorresult);
        await events.RaiseAsync(new UserLoginFailureEvent(request?.IdentityId, response.Message, false));
    }
}
