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
using designsphere_identity.STS.Identity.Dtos;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Linq;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;

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
    private readonly IConfiguration configuration;
    private readonly IEventService events;

    public UserInfoController(IUserService userService,
        IBus messageBroker,
        IExExUserIdentityRepository exExUserIdentityRepository,
        UserManager<UserIdentity> userManager,
        IUserDetailRepository userDetailRepository,
        IConfiguration configuration,
                IEventService events)
    {
        this.userService = userService;
        this.messageBroker = messageBroker;
        this.exExUserIdentityRepository = exExUserIdentityRepository;
        this.userManager = userManager;
        this.userDetailRepository = userDetailRepository;
        this.configuration = configuration;
        this.events = events;
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

    [HttpPut("UpdateUserProfile")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> UpdateUserProfile([FromForm] UserProfileDto userProfile)
    {
        var userDetail = await userDetailRepository.GetDetailOfUser(userService.GetUserName());
        if (userDetail == null)
            return new ApiResponse(new System.Exception("User detail not found"));

        var user = await userManager.Users.FirstOrDefaultAsync(_ => _.Id == userDetail.UserIdentityId);


        if (userDetail == null)
            return new ApiResponse(new System.Exception("User detail not found"));

        userDetail.JobRoleId = userProfile.JobRoleId;
        userDetail.CountryId = userProfile.CountryId;
        userDetail.PostalCode = userProfile.PostalCode;
        userDetail.IsPrivacyPolicyAccepted = userProfile.IsPrivacyPolicyAccepted;
        userDetail.CompanyName = userProfile.CompanyName;
        await userDetailRepository.UpdateAsync(userDetail);
        user.NewsLetterSubscription = user.NewsLetterSubscription;

        if (userProfile.ProfilePicture != null && userProfile.ProfilePicture.Length > 0)
        {
            // Read base path from configuration
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), configuration["AppSettings:ProfilePicturePath"]);

            // Generate a filename with the username
            var username = userService.GetUserName().Replace(" ", "_"); // Replace spaces with underscores
            var fileExtension = Path.GetExtension(userProfile.ProfilePicture.FileName);
            var fileName = $"{username}{fileExtension}";
            var savePath = Path.Combine(basePath, fileName);

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Save the file
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await userProfile.ProfilePicture.CopyToAsync(fileStream);
            }

            // Save the relative path in the database
            user.ProfilePictureUrl = $"/{configuration["AppSettings:ProfilePicturePath"].TrimStart('/')}/{fileName}";
        }
        await userManager.UpdateAsync(user);

        return new ApiResponse($"Record Udpated Sucessfully", true);
    }
    [HttpGet("GetUserDetails")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> GetUserDetails()
    {
        var id = userService.GetUserIdentity();
        var user = await userManager.Users.FirstOrDefaultAsync(_ => _.Id == id);

        var userDetail = await userDetailRepository.GetDetailOfUser(userService.GetUserName());


        // Handle Profile Picture
        var profilePictureDto = new UserPicture();
        var basePath = configuration["AppSettings:ProfilePicturePath"];
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), basePath);
        var imageFile = Path.Combine(directoryPath, $"{user.UserName.Replace(" ", "_")}{Path.GetExtension(user.ProfilePictureUrl)}");
        var profilePicturePath = Path.Combine(Directory.GetCurrentDirectory(), user.ProfilePictureUrl.TrimStart('/'));

        if (System.IO.File.Exists(imageFile))
        {
            try
            {
                // Read the image file into a byte array
                byte[] fileContents = await System.IO.File.ReadAllBytesAsync(imageFile);

                // Assign the ProfilePictureDto with the file data
                profilePictureDto.FileName = Path.GetFileName(imageFile);
                profilePictureDto.File = new FormFile(new MemoryStream(fileContents), 0, fileContents.Length, "file", Path.GetFileName(imageFile))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg", // Adjust the content type based on the actual image type
                    ContentDisposition = $"inline; filename={Path.GetFileName(imageFile)}"
                };
                profilePictureDto.bytes = fileContents;
            }
            catch (Exception ex)
            {
                return new ApiResponse($"Error reading file: {ex.Message}", false);
            }
        }

        var countries = await userDetailRepository.GetCountries();
        var jobFunctions = await userDetailRepository.GetJobRoles();

        var userDto = new UserDetailDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNo = user.PhoneNumber,
            CompanyName = userDetail.CompanyName,
            Country = countries.FirstOrDefault(c => c.Id == userDetail.CountryId)?.Name,
            JobFunction = jobFunctions.FirstOrDefault(c => c.Id == userDetail.JobRoleId)?.Name,
            PostalCode = userDetail.PostalCode,
            IsEmailMFA = userDetail.IsEmailMFA,
            IsGoogleMFA = userDetail.IsGoogleMFA,
            ProfilePicture = profilePictureDto
        };

        return new ApiResponse("get user details", userDto); 
    }

    [HttpGet("GetCountriesAndJobRoles")]
    [ProducesResponseType(typeof(CountryAndJobroleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> GetCountriesAndJobRoles()
    {
        try
        {
            var countries = await userDetailRepository.GetCountries();
            var jobRoles = await userDetailRepository.GetJobRoles();

            var result = new CountryAndJobroleDto
            {
                Countries = countries.Select(c => new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
                JobRoles = jobRoles.Select(j => new JobRoleDto
                {
                    Id = j.Id,
                    Name = j.Name
                }).ToList()
            };

            var response = new ApiResponse("Countries and jobRoles",result);
            
            return response;
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex);

        }
    }

    [HttpPost("ResetPassword")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> ResetPassword([FromForm]ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        var isOldPasswordValid = await userManager.CheckPasswordAsync(user, request.OldPassword);
        if (!isOldPasswordValid)
        {
            return new ApiResponse("Invalid old password.");
        }

        var resetPasswordResult = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!resetPasswordResult.Succeeded)
        {
            var errors = string.Join(", ", resetPasswordResult.Errors.Select(e => e.Description));
            return new ApiResponse($"Password reset failed: {errors}");
        }

        return new ApiResponse("Password reset successful.", true);

    }

    [HttpPost("LogOut")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse> LogOut()
    {
        try
        {
            // Sign the user out
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            await HttpContext.SignOutAsync();

            await events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

            return new ApiResponse("User logged out successfully.", true);
        }
        catch (Exception ex)
        {
            return new ApiResponse($"An error occurred during logout: {ex.Message}");
        }

    }
}

public class UpdateInfoDto : IName
{
    public string Name { get; set; }
}
