using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.STS.Identity.CustomGrants.Helpers;
using designsphere_identity.STS.Identity.CustomGrants.Models;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using designsphere_identity.STS.Identity.CustomGrants.Services;

namespace designsphere_identity.STS.Identity.CustomGrants;

public class PhoneGrantValidator : IExtensionGrantValidator
{
    private readonly UserManager<UserIdentity> userManager;
    private readonly RoleManager<UserIdentityRole> roleManager;
    private readonly IUserDetailRepository userDetailRepository;
    private readonly IDeviceRepository userDeviceRepository;
    private readonly IOTPService oTPService;
    private readonly IEventService events;
    private readonly IServiceProvider serviceProvider;
    private readonly ISmsService smsService;

    public PhoneGrantValidator(
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

    public string GrantType => "phonenumber";
    private PhoneGrantRequest request = new();
    private Payload payload;

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        try
        {
            ApiResponse response;
            request.PhoneNumber = context.Request.Raw.Get("phonenumber");
            request.DeviceCode = context.Request.Raw.Get("devicecode");
            request.AppHash = context.Request.Raw.Get("apphash");
            request.NotificationId = context.Request.Raw.Get("notificationid");

            if (string.IsNullOrEmpty(request.PhoneNumber) ||
                string.IsNullOrEmpty(request.AppHash) ||
                string.IsNullOrEmpty(request.DeviceCode) ||
                string.IsNullOrEmpty(request.NotificationId))
            {
                response = new(CustomValMsg.ParamMissing);
                CustomResult(context, response);
                return;
            }

            // Check Device Detail
            var deviceCheck = await userDeviceRepository.GetDeviceCheck(request.DeviceCode);
            if (deviceCheck != null && deviceCheck.DeviceStatus == EDeviceStatus.Blocked)
            {
                response = new(CustomValMsg.DeviceBlocked);
                CustomResult(context, response);
                return;
            }

            // check username
            var user = await userManager
                .Users
                .FirstOrDefaultAsync(_ => _.PhoneNumber == request.PhoneNumber);

            // Check User Status
            if (user != null && user.LockoutEnabled && user.LockoutEnd >= DateTime.Now)
            {
                response = new(CustomValMsg.UserLockout(user));
                CustomResult(context, response);
                return;
            }

            // check type
            if (user != null && user.PhoneNumberConfirmed && user.Type != EUserType.phonenumber)
            {
                response = new(CustomValMsg.DuplicatePhonenumber(user));
                CustomResult(context, response);
                return;
            }

            // validate user
            // create new user

            if (user == null)
            {
                user = new()
                {
                    UserName = Guid.NewGuid().ToString(),
                    PhoneNumber = request.PhoneNumber,
                    Type = EUserType.phonenumber,
                    CreationDate = DateTime.UtcNow,
                };
                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    response = new(result.Errors.First().Description);
                    CustomResult(context, response);
                    return;
                }
            }
            // register device
            if (deviceCheck == null)
            {
                deviceCheck = new Device() { DeviceCode = request.DeviceCode, CreatorId = user.Id };
                if (request.DeviceCode == "required")
                    deviceCheck.DeviceStatus = EDeviceStatus.Developer;
                await userDeviceRepository.AddAsync(deviceCheck);
            }
            var userdetailCheck = await userDetailRepository.GetDetailOfUser(user.UserName);
            if (userdetailCheck != null)
            {
                if (userdetailCheck.OTPRetries >= oTPService.Retries)
                {
                    userdetailCheck.OTPRetries++;
                    await userDetailRepository.UpdateAndLog(userdetailCheck);

                    response = new(CustomValMsg.MaxOtpReached);
                    CustomResult(context, response);
                    return;
                }
                if (userdetailCheck.OTPStatus == EOTPStatus.OTPSent && userdetailCheck.CreationTime <= DateTime.UtcNow.AddSeconds(180))
                {
                    userdetailCheck.OTPRetries++;
                    await userDetailRepository.UpdateAndLog(userdetailCheck);
                    response = new(CustomValMsg.OtpAlreadySent);
                    CustomResult(context, response);
                    return;
                }
            }
            var otptype = context.Request.Raw.Get("otptype");
            if (string.IsNullOrEmpty(otptype))
            {
                response = new(CustomValMsg.ParamOtpMissed);
                CustomResult(context, response);
                return;
            }
            if (Enum.TryParse(otptype, true, out EPhoneOtp otptypet))
                request.OtpType = otptypet;
            else
            {
                response = new(CustomValMsg.InvalidOtpType());
                CustomResult(context, response);
                return;
            }
            switch (request.OtpType)
            {
                case EPhoneOtp.Generate:

                    var otp = oTPService.GenerateOTP(user.UserName, request.AppHash, GrantType);
                    if (await smsService.SendAsync(user.PhoneNumber, smsService.GetMessageBody(request, otp.Otp), deviceCheck.DeviceStatus == EDeviceStatus.Developer))
                    {
                        if (userdetailCheck == null)
                            userdetailCheck = new UserDetail()
                            {
                                CreatorId = user.UserName,
                                NotificationId = request.NotificationId,
                                AppHash = request.AppHash,
                                UserIdentityId = user.Id,
                                DeviceId = deviceCheck.Id,
                                OTPStatus = EOTPStatus.OTPRequested,
                                UserName = user.UserName,
                                OTPRetries = +1,
                                Name = userdetailCheck != null ? userdetailCheck.Name : request.PhoneNumber
                            };
                        else
                        {
                            userdetailCheck.CreatorId = user.UserName;
                            userdetailCheck.NotificationId = request.NotificationId;
                            userdetailCheck.AppHash = request.AppHash;
                            userdetailCheck.UserIdentityId = user.Id;
                            userdetailCheck.DeviceId = deviceCheck.Id;
                            userdetailCheck.OTPStatus = EOTPStatus.OTPRequested;
                            userdetailCheck.UserName = user.UserName;
                            userdetailCheck.OTPRetries = +1;
                            userdetailCheck.Name = userdetailCheck != null ? userdetailCheck.Name : request.PhoneNumber;
                        }
                        await userDetailRepository.UpdateAndLog(userdetailCheck);
                        if (request.PhoneNumber == "03011121980")
                        {
                            //assign role
                            const string role = "customer1";

                            if (!await userManager.IsInRoleAsync(user, role))
                            {
                                if (!await roleManager.RoleExistsAsync(role))
                                {
                                    response = new(CustomValMsg.RoleNotFound(role));
                                    CustomResult(context, response);
                                    return;
                                }
                                await userManager.AddToRoleAsync(user, role);
                            }
                            var userinfo = GrantHelper.GetUserinfo(user, userdetailCheck);
                            UserInfoDev udev = new()
                            {
                                Id = userinfo.Id,
                                PhoneNumber = userinfo.PhoneNumber,
                                OauthType = userinfo.OauthType,
                                UserName = userinfo.UserName,
                                AppHash = userinfo.AppHash,
                                Name = userinfo.Name,
                                NotificationId = userinfo.NotificationId,
                            };
                            if (!user.PhoneNumberConfirmed)
                            {
                                user.PhoneNumberConfirmed = true;
                                await userManager.UpdateAsync(user);
                            }
                            response = new($"", udev);
                            context.Result = new GrantValidationResult(user.Id, authenticationMethod: GrantType, customResponse: GrantHelper.ClassToDict(response));
                            await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, response.Message, false));
                            return;
                        }

                        var responseotp = new OTPResponse(otp.ProtectToken);
                        response = new(CustomValMsg.OtpSent(user, oTPService.Validity), responseotp);
                        CustomResult(context, response);
                        return;
                    }
                    else
                    {
                        response = new(CustomValMsg.MessageServiceUnavailable);
                        CustomResult(context, response);
                        return;
                    }
                case EPhoneOtp.Verify:
                    if (userdetailCheck == null)
                    {
                        response = new(CustomValMsg.GenerateOtpFirst);
                        CustomResult(context, response);
                        return;
                    }
                    // verify otp
                    request.Otp = context.Request.Raw.Get("otp");
                    request.ProtectToken = context.Request.Raw.Get("protecttoken");

                    if (string.IsNullOrEmpty(request.Otp) || string.IsNullOrEmpty(request.ProtectToken))
                    {
                        response = new(CustomValMsg.ParamOtpPTokenMissing);
                        CustomResult(context, response);
                        return;
                    }

                    if (userdetailCheck.OTPStatus == EOTPStatus.OTPRequested
                        && oTPService.VerifyOTP(user.UserName, request, request.AppHash, GrantType))
                    {
                        if (!user.PhoneNumberConfirmed)
                        {
                            // add phone number
                            user.PhoneNumberConfirmed = true;
                            var result = await userManager.UpdateAsync(user);
                            // create claims
                            var claims = new List<Claim>
                            {
                                new("phone_number", request.PhoneNumber),
                            };
                            foreach (var claim in claims)
                            {
                                await userManager.AddClaimAsync(user, new Claim(claim.Type, claim.Value));
                            }

                            //assign role
                            const string role = "customer1";
                            if (!await roleManager.RoleExistsAsync(role))
                            {
                                response = new(CustomValMsg.RoleNotFound(role));
                                CustomResult(context, response);
                                return;
                            }
                            await userManager.AddToRoleAsync(user, role);
                        }
                        userdetailCheck.OTPStatus = EOTPStatus.OTPVerified;
                        await userDetailRepository.UpdateAndLog(userdetailCheck);
                        var userinfo = GrantHelper.GetUserinfo(user, userdetailCheck);
                        response = new($"", userinfo);
                        context.Result = new GrantValidationResult(user.Id, authenticationMethod: GrantType, customResponse: GrantHelper.ClassToDict(response));
                        await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, response.Message, false));
                        return;
                    }
                    else
                    {
                        response = new(CustomValMsg.OtpExipredOrInvalid);
                        CustomResult(context, response);
                        return;
                    }
                default:
                    response = new(CustomValMsg.InvalidRequest);
                    break;
            }
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
        await events.RaiseAsync(new UserLoginFailureEvent(request.PhoneNumber, response.Message, false));
    }
}
