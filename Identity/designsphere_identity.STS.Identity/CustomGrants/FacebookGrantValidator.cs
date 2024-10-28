using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.STS.Identity.CustomGrants.Helpers;
using designsphere_identity.STS.Identity.CustomGrants.Models;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using designsphere_identity.STS.Identity.CustomGrants.Services;

namespace designsphere_identity.STS.Identity.CustomGrants;

public class FacebookGrantValidator : IExtensionGrantValidator

{
    private readonly UserManager<UserIdentity> userManager;
    private readonly RoleManager<UserIdentityRole> roleManager;
    private readonly IUserDetailRepository userDetailRepository;
    private readonly IDeviceRepository userDeviceRepository;
    private readonly IEventService events;
    private readonly ISmsService smsService;
    private readonly IOTPService oTPService;

    private readonly IConfiguration configuration;

    public FacebookGrantValidator(
        UserManager<UserIdentity> userManager,
        RoleManager<UserIdentityRole> roleManager,
        IUserDetailRepository userDetailRepository,
        IDeviceRepository userDeviceRepository,
        IEventService events,
        IOTPService oTPService,
        ISmsService smsService,
        IConfiguration configuration
        )
    {
        this.userDetailRepository = userDetailRepository;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.userDeviceRepository = userDeviceRepository;
        this.events = events;
        this.smsService = smsService;
        this.configuration = configuration;
        this.oTPService = oTPService;
    }

    public string GrantType => "facebook";

    private PhoneGrantRequest request = new();
    private SocialTokenValidationResult payload;

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        try
        {
            ApiResponse response;
            request.DeviceCode = context.Request.Raw.Get("devicecode");
            request.NotificationId = context.Request.Raw.Get("notificationid");
            request.AppHash = context.Request.Raw.Get("apphash");
            request.IdToken = context.Request.Raw.Get("idtoken");

            if (string.IsNullOrEmpty(request.IdToken) ||
                string.IsNullOrEmpty(request.DeviceCode) ||
                string.IsNullOrEmpty(request.AppHash) ||
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
            switch (request.IdToken)
            {
                default:
                    break;
            }
            // verify facebook token
            try
            {
                var clientid = configuration["SocialLogin:Facebook:ClientId"];
                //// mock facebook token
                //var payloadsc = new FacebookTokenValidationResult()
                //{
                //    Data = new FacebookTokenValidationData()
                //    {
                //        AppId = clientid,
                //        Application = clientid,
                //        UserId = "newfacebookuserid"
                //    }
                //};

                var payloadsc = await ValidateFacebookToken(request.IdToken);
                if (payloadsc.Data.AppId != clientid)
                {
                    response = new(CustomValMsg.InvalidAudience);
                    CustomResult(context, response);
                    return;
                }
                if (!payloadsc.Data.IsValid)
                {
                    response = new(CustomValMsg.InvalidIdToken);
                    CustomResult(context, response);
                    return;
                }
                payload = new SocialTokenValidationResult()
                {
                    UserId = payloadsc.Data.UserId,
                    AudienceId = payloadsc.Data.AppId,
                    IsValid = payloadsc.Data.IsValid,
                    Name = payloadsc.Data.Name,
                };
            }
            catch (Exception ex)
            {
                response = new(ex.Message);
                CustomResult(context, response);
                return;
            }

            // check username
            var user = await userManager.FindByNameAsync(payload.UserId);
            // Check User Status
            if (user != null && user.LockoutEnabled && user.LockoutEnd >= DateTime.Now)
            {
                response = new(CustomValMsg.UserLockout(user));
                CustomResult(context, response);
                return;
            }

            if (user != null && user.PhoneNumberConfirmed && user.Type == EUserType.facebook)
            {
                var userdetailCheck1 = await userDetailRepository.GetDetailOfUser(user.UserName);
                var userinfo = GrantHelper.GetUserinfo(user, userdetailCheck1);
                response = new($"Sucessful", userinfo);
                context.Result = new GrantValidationResult(user.Id, authenticationMethod: GrantType, customResponse: GrantHelper.ClassToDict(response));
                return;
            }

            // validate user
            // create new user
            if (user == null)
            {
                user = new()
                {
                    UserName = payload.UserId,
                    Type = EUserType.facebook,
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

            request.PhoneNumber = context.Request.Raw.Get("phonenumber");

            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                response = new(CustomValMsg.ParamMissing, new OauthResult(false, ""), true);
                CustomResult(context, response);
                return;
            }
            // check phonenumber
            var userbyphone = userManager.Users.FirstOrDefault(_ => _.PhoneNumber == request.PhoneNumber);
            if (userbyphone != null && userbyphone.PhoneNumberConfirmed && userbyphone.UserName != user.UserName)
            {
                response = new(CustomValMsg.DuplicatePhonenumber(userbyphone, request.PhoneNumber));
                CustomResult(context, response);
                return;
            }

            var userdetailCheck = await userDetailRepository.GetDetailOfUser(user.UserName);

            if (userdetailCheck != null)
            {
                if (userdetailCheck.OTPRetries >= oTPService.Retries)
                {
                    userdetailCheck.OTPRetries++;
                    await userDetailRepository.AddAsync(userdetailCheck);

                    response = new(CustomValMsg.MaxOtpReached);
                    CustomResult(context, response);
                    return;
                }
                if (userdetailCheck.OTPStatus == EOTPStatus.OTPSent && userdetailCheck.CreationTime <= DateTime.UtcNow.AddSeconds(180))
                {
                    userdetailCheck.OTPRetries++;
                    await userDetailRepository.AddAsync(userdetailCheck);
                    response = new(CustomValMsg.OtpAlreadySent);
                    CustomResult(context, response);
                    return;
                }
            }

            switch (request.OtpType)
            {
                case EPhoneOtp.Generate:
                    var otp = oTPService.GenerateOTP(user.UserName, request.AppHash, GrantType);
                    if (!user.PhoneNumberConfirmed)
                    {
                        user.PhoneNumber = request.PhoneNumber;
                        await userManager.UpdateAsync(user);
                    }
                    if (await smsService.SendAsync(request.PhoneNumber, smsService.GetMessageBody(request, otp.Otp), deviceCheck.DeviceStatus == EDeviceStatus.Developer))
                    {
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
                            Name = payload.Name,
                        };
                        await userDetailRepository.AddAsync(userdetailCheck);
                        var responseotp = new OauthResult(true, otp.ProtectToken);
                        response = new(CustomValMsg.OtpSent(user, 180), responseotp);
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

                    if (userdetailCheck.OTPStatus == EOTPStatus.OTPRequested && oTPService.VerifyOTP(user.UserName, request, request.AppHash, GrantType))
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
                        await userDetailRepository.UpdateAsync(userdetailCheck);
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

    private async Task<FacebookTokenValidationResult> ValidateFacebookToken(string idtoken)
    {
        var httpClient = new HttpClient();
        var appAccessTokenResponse = await httpClient.GetFromJsonAsync<FacebookAppAccessTokenResponse>($"https://graph.facebook.com/oauth/access_token?client_id={configuration["SocialLogin:Facebook:ClientId"]}&client_secret={configuration["SocialLogin:Facebook:ClientSecret"]}&grant_type=client_credentials");
        var res1 = await httpClient.GetFromJsonAsync<FacebookTokenValidationResult>(
                $"https://graph.facebook.com/debug_token?input_token={idtoken}&access_token={appAccessTokenResponse!.AccessToken}");
        var res2 = await httpClient.GetFromJsonAsync<FacebookIdTokenValidationData>(
                   $"https://graph.facebook.com/me?access_token={idtoken}");
        res1.Data.Name = res2.Name;
        return res1;
    }

    //private async Task<FacebookIdTokenValidationData> ValidateFacebookIdToken(string idtoken)
    //{
    //    var httpClient = new HttpClient();
    //    var appAccessTokenResponse = await httpClient.GetFromJsonAsync<FacebookAppAccessTokenResponse>($"https://graph.facebook.com/oauth/access_token?client_id={configuration["SocialLogin:Facebook:ClientId"]}&client_secret={configuration["SocialLogin:Facebook:ClientSecret"]}&grant_type=client_credentials");

    //}
}

public class FacebookAppAccessTokenResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }
}

public class FacebookTokenValidationResult
{
    [JsonPropertyName("data")] public FacebookTokenValidationData Data { get; set; } = null!;
}

public class FacebookIdTokenValidationData
{
    [JsonPropertyName("id")] public string UserId { get; set; } = null!;
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
}

public class SocialTokenValidationResult
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string AudienceId { get; set; }
    public bool IsValid { get; set; }
}

public class FacebookTokenValidationData
{
    [JsonPropertyName("app_id")] public string AppId { get; set; } = null!;

    [JsonPropertyName("type")] public string Type { get; set; } = null!;

    [JsonPropertyName("application")] public string Application { get; set; } = null!;

    [JsonPropertyName("data_access_expires_at")] public int DataAccessExpiresAt { get; set; }

    [JsonPropertyName("expires_at")] public int ExpiresAt { get; set; }

    [JsonPropertyName("is_valid")] public bool IsValid { get; set; }

    [JsonPropertyName("scopes")] public List<string> Scopes { get; set; } = null!;

    [JsonPropertyName("user_id")] public string UserId { get; set; } = null!;
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
}
