using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using designsphere_identity.STS.Identity.CustomGrants.Models;
using designsphere_identity.STS.Identity.CustomGrants.Helpers;
using Microsoft.EntityFrameworkCore;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.STS.Identity.CustomGrants.Repository;
using designsphere_identity.STS.Identity.CustomGrants.Services;
using OtpNet;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace designsphere_identity.STS.Identity.CustomGrants;

public class EmailGrantValidator : IExtensionGrantValidator
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly RoleManager<UserIdentityRole> _roleManager;
    private readonly IUserDetailRepository _userDetailRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IConfiguration _configuration;
    private readonly IEventService _events;
    private readonly IOTPService oTPService;
    private readonly IEmailService emailService;



    public EmailGrantValidator(
        UserManager<UserIdentity> userManager,
        RoleManager<UserIdentityRole> roleManager,
        IUserDetailRepository userDetailRepository,
        IDeviceRepository deviceRepository,
        IConfiguration configuration,
        IEventService events,
        IOTPService oTPService,
        IEmailService emailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userDetailRepository = userDetailRepository;
        _deviceRepository = deviceRepository;
        _configuration = configuration;
        _events = events;
        this.oTPService = oTPService;
        this.emailService = emailService;
    }

    public string GrantType => "emailpassword";
    private EmailGrantRequest request = new();


    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        try
        {
            ApiResponse response;
            request.Email = context.Request.Raw.Get("email");
            request.Password = context.Request.Raw.Get("password");
            request.FirstName = context.Request.Raw.Get("firstName");
            request.LastName = context.Request.Raw.Get("lastName");
            request.DeviceCode = context.Request.Raw.Get("devicecode");
            request.ForgotPassword = context.Request.Raw.Get("forgotpassword") == "1";
            request.Otp = context.Request.Raw.Get("otp");
            request.ProtectToken = context.Request.Raw.Get("protecttoken");

            var otptype = context.Request.Raw.Get("otptype");

            var user = await _userManager.Users.FirstOrDefaultAsync(_ => _.Email == request.Email);

            if (request.ForgotPassword)
            {
                // Forgot Password Flow
                if (string.IsNullOrEmpty(request.Email))
                {
                    response = new("Email is required for forgot password.");
                    CustomResult(context, response);
                    return;
                }

                if (user == null)
                {
                    response = new("User with the given email does not exist.");
                    CustomResult(context, response);
                    return;
                }

                var userdetail = await _userDetailRepository.GetDetailOfUser(user.UserName);

                // OTP Verification
                if (!string.IsNullOrEmpty(request.Otp) && !string.IsNullOrEmpty(request.ProtectToken))
                {

                    if (userdetail == null || userdetail.OTPStatus != EOTPStatus.OTPRequested)
                    {
                        response = new("Please generate OTP first.");
                        CustomResult(context, response);
                        return;
                    }

                   if (userdetail.OTPStatus == EOTPStatus.OTPRequested
                        || oTPService.VerifyOTP(user.UserName, request, "testhash", GrantType))
                        {
                            // Reset Password
                            if (string.IsNullOrEmpty(request.Password))
                        {
                            response = new("New password is required to reset password.");
                            CustomResult(context, response);
                            return;
                        }

                        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, request.Password);

                        if (!resetResult.Succeeded)
                        {
                            response = new(resetResult.Errors.First().Description);
                            CustomResult(context, response);
                            return;
                        }

                        userdetail.OTPStatus = EOTPStatus.OTPVerified;
                        await _userDetailRepository.UpdateAndLog(userdetail);

                        response = new("Password has been reset successfully.");
                        CustomResult(context, response);
                        return;
                    }
                    else
                    {
                        response = new("Invalid or expired OTP.");
                        CustomResult(context, response);
                        return;
                    }
                }
                else
                {
                    // Generate OTP
                    var forgotOtpResponse = await GenerateOtpApiCall(user.Email);

                    if (forgotOtpResponse.StatusCode == 200)
                    {
                        userdetail ??= new UserDetail
                        {
                            CreatorId = user.UserName,
                            UserIdentityId = user.Id,
                            OTPStatus = EOTPStatus.OTPRequested,
                            UserName = user.UserName,
                            OTPRetries = 1,
                            Name = user.FirstName
                        };

                        userdetail.OTPStatus = EOTPStatus.OTPRequested;
                        userdetail.OTPRetries += 1;

                        await _userDetailRepository.UpdateAndLog(userdetail);

                        var apiResponse = JsonConvert.DeserializeObject<JObject>(forgotOtpResponse.Message);
                        var result = apiResponse["result"] as JObject;
                        var otp = result["otp"]?.ToString();
                        var protectToken = result["protectToken"]?.ToString();

                        response = new(CustomValMsg.OtpSent(user, oTPService.Validity), protectToken);
                        CustomResult(context, response);
                        return;
                    }
                    else
                    {
                        response = new(CustomValMsg.MessageServiceUnavailable);
                        CustomResult(context, response);
                        return;
                    }
                }
            }

            if ((string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password)) && request.OtpType == EPhoneOtp.Generate)
            {
                response = new(CustomValMsg.ParamMissing);
                CustomResult(context, response);
                return;
            }

            var deviceCheck = await _deviceRepository.GetDeviceCheck(request.DeviceCode);
            if (deviceCheck != null && deviceCheck.DeviceStatus == EDeviceStatus.Blocked)
            {
                response = new(CustomValMsg.DeviceBlocked);
                CustomResult(context, response);
                return;
            }
            else if(deviceCheck == null && (request.DeviceCode != null))
            {
                // If the device does not exist in the database, create a new device
                var newDevice = new Device
                {
                    DeviceCode = request.DeviceCode,
                    DeviceStatus = EDeviceStatus.Allow
                };

                await _deviceRepository.AddAsync(newDevice);

                deviceCheck = await _deviceRepository.GetDeviceCheck(request.DeviceCode);
            }

            if (user != null && request.OtpType == EPhoneOtp.Generate)
            {
                if (user != null && !await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    response = new(CustomValMsg.InvalidEmailPassword);
                    CustomResult(context, response);
                    return;
                }

                if (user != null && user.LockoutEnabled && user.LockoutEnd >= DateTime.Now)
                {
                    response = new(CustomValMsg.UserLockout(user));
                    CustomResult(context, response);
                    return;
                }

            }
            // Handle new registration
            if (user == null && (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName)))
            {
                response = new(CustomValMsg.UserNotExist);
                CustomResult(context, response);
                return;
            }

            if (user == null)
            {
                if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName))
                {
                    response = new("First name and last name are required for new users.");
                    CustomResult(context, response);
                    return;
                }

                user = new UserIdentity
                {
                    UserName = Guid.NewGuid().ToString(),
                    Email = request.Email,
                    EmailConfirmed = true,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                };

                var createUserResult = await _userManager.CreateAsync(user, request.Password);
                if (!createUserResult.Succeeded)
                {
                    response = new(createUserResult.Errors.First().Description);
                    CustomResult(context, response);
                    return;
                }
            }

            var userdetailCheck = await _userDetailRepository.GetDetailOfUser(user.UserName);
                if (userdetailCheck != null)
                {
                    // Validate OTP retries and status
                    response = await oTPService.ValidateOTPAsync(user.UserName);
                    if (response != null)
                    {
                        CustomResult(context, response);
                        return;
                    }

                    response = await oTPService.ValidateOTPSentStatusAsync(user.UserName);
                    if (response != null)
                    {
                        CustomResult(context, response);
                        return;
                    }
                }


           
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
                    //var otpCode = oTPService.GenerateOTP(user.UserName, "", GrantType);
                    var otpResponse = await GenerateOtpApiCall(user.Email);

                    //  bool emailSent = await emailService.SendEmailAsync(request.Email, "verification email", emailService.GetMessageBody(request, otpCode.Otp));


                    //if (!emailSent)
                    //{
                    //    response = new("Failed to send OTP to the provided email.");
                    //    CustomResult(context, response);
                    //    return;
                    //}
                    //if (!otpResponse)
                    //{
                    //    response = new(CustomValMsg.MessageServiceUnavailable);
                    //    CustomResult(context, response);
                    //    return;
                    //}

                  //  if (await emailService.SendEmailAsync(request.Email, "verification email", emailService.GetMessageBody(request, otpCode.Otp)))
                        if (otpResponse.StatusCode == 200)

                        {

                        if (userdetailCheck == null)
                            userdetailCheck = new UserDetail()
                            {
                                CreatorId = user.UserName,
                                UserIdentityId = user.Id,
                                OTPStatus = EOTPStatus.OTPRequested,
                                UserName = user.UserName,
                                OTPRetries = +1,
                                Name = user.FirstName,
                                DeviceId = deviceCheck.Id,
                            };
                        else
                        {
                            userdetailCheck.CreatorId = user.UserName;
                            userdetailCheck.UserIdentityId = user.Id;
                            userdetailCheck.OTPStatus = EOTPStatus.OTPRequested;
                            userdetailCheck.UserName = user.UserName;
                            userdetailCheck.OTPRetries = +1;
                            userdetailCheck.Name = user.FirstName;
                            userdetailCheck.DeviceId = deviceCheck.Id;
                        }
                        await _userDetailRepository.UpdateAndLog(userdetailCheck);

                        var apiResponse = JsonConvert.DeserializeObject<JObject>(otpResponse.Message);

                        //var responseotp = new OTPResponse(otpCode.ProtectToken);
                        var result = apiResponse["result"] as JObject;

                        var otp = result["otp"]?.ToString();
                        var protectToken = result["protectToken"]?.ToString();

                        response = new(CustomValMsg.OtpSent(user, oTPService.Validity), protectToken);
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
                     || oTPService.VerifyOTP(user.UserName, request, "testhash", GrantType))
                    {
                      //  if (!user.EmailConfirmed)
                       // {
                            // Mark email as confirmed after successful OTP validation
                            user.EmailConfirmed = true;
                            var result = await _userManager.UpdateAsync(user);

                        // create claims
                        const string role = "customer1";

                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.UserName),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(ClaimTypes.Role, role)

                            };
                            foreach (var claim in claims)
                            {
                                await _userManager.AddClaimAsync(user, new Claim(claim.Type, claim.Value));
                            }

                            //assign role
                            if (!await _roleManager.RoleExistsAsync(role))
                            {
                                response = new(CustomValMsg.RoleNotFound(role));
                                CustomResult(context, response);
                                return;
                            }
                            await _userManager.AddToRoleAsync(user, role);

                      //  }
                

                        userdetailCheck.OTPStatus = EOTPStatus.OTPVerified;

                        await _userDetailRepository.UpdateAndLog(userdetailCheck);
                        var userinfo = GrantHelper.GetUserinfo(user, userdetailCheck);
                        response = new($"", userinfo);
                        context.Result = new GrantValidationResult(user.Id, authenticationMethod: GrantType, customResponse: GrantHelper.ClassToDict(response));
                        await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, response.Message, false));
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

    private async void CustomResult(ExtensionGrantValidationContext context, ApiResponse response)
    {
        Dictionary<string, object> errorresult = GrantHelper.ClassToDict(response);
        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, customResponse: errorresult);
        await _events.RaiseAsync(new UserLoginFailureEvent("email", response.Message, false));
    }


    private async Task<ApiResponse> GenerateOtpApiCall(string email)
    {
        var devOtpDto = new Controllers.DevOtpDto
        {
            AppHash = "testhash", 
            GrantType = "emailpassword",
            Email = email
        };

    
            var queryParams = new Dictionary<string, string>
        {
            { "Email", devOtpDto.Email },
            { "AppHash", devOtpDto.AppHash },
            { "GrantType", devOtpDto.GrantType }
        };

        var baseUrl = _configuration["AdminConfiguration:IdentityServerBaseUrl"];

        var content = new FormUrlEncodedContent(queryParams);
        var queryString = await content.ReadAsStringAsync();

        var url = $"{baseUrl}/DevOtp?{queryString}";


        using (var client = new System.Net.Http.HttpClient())
        {
            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse(responseContent, true);
            }
            else
            {
                return new ApiResponse(responseContent, false);
            }
        }
    }

}