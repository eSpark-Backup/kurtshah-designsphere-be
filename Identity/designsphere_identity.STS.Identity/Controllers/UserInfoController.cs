using EventBus.Messages.Sts.Events;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.STS.Identity.CustomGrants.Models;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using designsphere_identity.STS.Identity.CustomGrants.Services;

namespace designsphere_identity.STS.Identity.Controllers;

[Route("[controller]")]
[Authorize(LocalApi.PolicyName)]
[ApiController]
public class UserInfoController : ControllerBase
{
    private readonly IUserService userService;
    private readonly IBus messageBroker;
    private readonly IExExUserIdentityRepository exExUserIdentityRepository;
    private readonly UserManager<UserIdentity> userManager;
    private readonly IUserDetailRepository userDetailRepository;

    public UserInfoController(IUserService userService,
        IBus messageBroker,
        IExExUserIdentityRepository exExUserIdentityRepository,
        UserManager<UserIdentity> userManager,
        IUserDetailRepository userDetailRepository)
    {
        this.userService = userService;
        this.messageBroker = messageBroker;
        this.exExUserIdentityRepository = exExUserIdentityRepository;
        this.userManager = userManager;
        this.userDetailRepository = userDetailRepository;
    }
    [HttpPost("[action]")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> DeleteUser([FromForm] string remarks)
    {
        var id = userService.GetUserIdentity();
        var user = await userManager.Users.FirstOrDefaultAsync(_ => _.Id == id);
        if (user == null)
            return new ApiResponse(new Exception("User not found"));

        var exUser = new ExUserIdentity()
        {
            Id = user.Id,
            Email = user.Email,
            AccessFailedCount = user.AccessFailedCount,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            Remarks = remarks,
            Type = user.Type,
            UserName = user.UserName,
            ConcurrencyStamp = user.ConcurrencyStamp,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            NormalizedEmail = user.NormalizedEmail,
            NormalizedUserName = user.NormalizedUserName,
            PasswordHash = user.PasswordHash,
            SecurityStamp = user.SecurityStamp,
            TwoFactorEnabled = user.TwoFactorEnabled,
            CreationDate = DateTime.UtcNow,
        };
        await exExUserIdentityRepository.AddUser(exUser);
        try
        {
            await userManager.DeleteAsync(user);
            return new ApiResponse($"User Deleted Sucessfully", true);
        }
        catch (Exception ex)
        {
            exExUserIdentityRepository.DeleteUser(exUser);
            return new ApiResponse(ex);
        }
    }
    [HttpPost]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> Update([FromForm] UpdateInfoDto model)
    {
        var userDetail = await userDetailRepository.GetDetailOfUser(userService.GetUserName());
        if (userDetail == null)
            return new ApiResponse(new System.Exception("User detail not found"));
        userDetail.Name = model.Name;
        await userDetailRepository.UpdateAsync(userDetail);
        var uupdateEvent = new UserInfoUpdateEvent()
        {
            ClientId = "designsphere",
            CreatorId = userDetail.UserIdentityId,
            CreatorName = userDetail.Name,
            FQDN = "designsphere.io",
            Name = userDetail.Name,
            UserId = userDetail.UserIdentityId
        };
        await messageBroker.Publish(uupdateEvent);
        return new ApiResponse($"Record Udpated Sucessfully", true);
    }
}

public class UpdateInfoDto : IName
{
    public string Name { get; set; }
}
