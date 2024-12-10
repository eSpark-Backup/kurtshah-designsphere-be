using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using designsphere_identity.STS.Identity.CustomGrants.Models;
using designsphere_identity.STS.Identity.CustomGrants.Helpers;
using Microsoft.EntityFrameworkCore;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.STS.Identity.CustomGrants.Services;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using System.Web;
using designsphere_identity.STS.Identity.Migrations;

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

    public string? otp;
    public string? protectToken;


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
            request.Token = context.Request.Raw.Get("token");

            var otptype = context.Request.Raw.Get("otptype");

            var user = await _userManager.Users.FirstOrDefaultAsync(_ => _.Email == request.Email);


            if(request.ForgotPassword)
            {
                if (user != null && user.Email.ToUpper() != request.Email.ToUpper() )
                {
                    response = new(CustomValMsg.EmailNotCorrect);
                    CustomResult(context, response);
                    return;
                }
                if (otptype == "Generate")
                {
                    var forgotPasswordResponse = await GenerateForgotPasswordTokenAsync(user);

                    if (forgotPasswordResponse.IsError == true)
                    {
                        CustomResult(context, forgotPasswordResponse);
                        return;
                    }

                    CustomResult(context, forgotPasswordResponse);
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Password))
                    {
                        response = new(CustomValMsg.ParamMissing);
                        CustomResult(context, response);
                        return;
                    }

                    var result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(request.Token), request.Password);

                    if (!result.Succeeded)
                    {
                        response = (new ApiResponse(CustomValMsg.PasswordResetFailed));
                        CustomResult(context, response);
                        return;
                    }

                    response = (new ApiResponse("Password reset successfully.",null));
                    response.IsError = false;
                    CustomResult(context, response);
                    return;

                }

            }
            // Check if user already exists with the same email and FirstName is provided (and OTP Type is Generate)
            if (user != null && user.Email.ToUpper() == request.Email.ToUpper() && !string.IsNullOrEmpty(request.FirstName) && request.OtpType == EPhoneOtp.Generate)
            {
                response = new(CustomValMsg.UserExist);
                CustomResult(context, response);
                return;
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
            else if (deviceCheck == null && (request.DeviceCode != null))
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

            if (user != null && !string.IsNullOrEmpty( request.Password))
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
            if (user == null && (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName)))
            {
                response = new(CustomValMsg.UserNotExist);
                CustomResult(context, response);
                return;
            }

            if (user == null)
            {
                // Handle new registration logic
                if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName))
                {
                    response = new ApiResponse("First name and last name are required for new users.");
                    CustomResult(context, response);
                    return;
                }
                var newUser = new UserIdentity
                {
                    UserName = Guid.NewGuid().ToString(),
                    Email = request.Email,
                    EmailConfirmed = false,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Type = EUserType.email

                };

                var createdUser = await _userManager.CreateAsync(newUser, request.Password);
                if (!createdUser.Succeeded)
                {
                    response = new ApiResponse("user not created");
                    CustomResult(context, response);
                    return;
                }
                bool IsEmailDelivered = await SendOtpAsync(user);


                if (createdUser != null && IsEmailDelivered)
                {
                       var userdetails = new UserDetail()
                        {
                            CreatorId = newUser.UserName,
                            UserIdentityId = newUser.Id,
                            OTPStatus = EOTPStatus.OTPRequested,
                            UserName = newUser.UserName,
                            OTPRetries = +1,
                            Name = newUser.FirstName,
                            DeviceId = deviceCheck.Id,
                        };
                    
                    await _userDetailRepository.UpdateAndLog(userdetails);
                    response = new ApiResponse("check Email to verify the account",null);
                    response.Result = new
                    {
                        ProtectToken = protectToken
                    };
                    CustomResult(context, response);
                    return ;
                }
                CustomResult(context, new ApiResponse(CustomValMsg.InvalidRequest));
                return;
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
            if (!user.EmailConfirmed && !string.IsNullOrEmpty(request.FirstName))
            {
                response = new(CustomValMsg.UserNotVerified);
                CustomResult(context, response);
                return;
            }
            else if (!user.EmailConfirmed &&  otptype != "Verify")
            {
                bool IsEmailDelivered = await SendOtpAsync(user);

                if (IsEmailDelivered)
                {
                    userdetailCheck.OTPStatus = EOTPStatus.OTPRequested;
                    userdetailCheck.OTPRetries = +1;

                    await _userDetailRepository.UpdateAndLog(userdetailCheck);
                    response = new ApiResponse("check Email to verify the account", null);
                    response.Result = new
                    {
                        ProtectToken = protectToken
                    };
                    CustomResult(context, response);
                    return;
                }
                CustomResult(context, new ApiResponse(CustomValMsg.InvalidRequest));
                return;

            }
            else if(user.EmailConfirmed)
            {
                //if(userdetailCheck.IsEmailMFA == true)
                //{
                //    bool IsEmailDelivered = await SendOtpAsync(user);
                //    response = new("verify account through email");
                //    CustomResult(context, response);
                //    return;
                //}
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



                await _userDetailRepository.UpdateAndLog(userdetailCheck);
                var userinfo = GrantHelper.GetUserinfo(user, userdetailCheck);
                response = new($"", userinfo);
                context.Result = new GrantValidationResult(user.Id, authenticationMethod: GrantType, customResponse: GrantHelper.ClassToDict(response));
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, response.Message, false));
                return;
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

                    if (userdetailCheck.OTPStatus == EOTPStatus.OTPRequested &&
                        oTPService.VerifyOTP(user.UserName, request, "testhash", GrantType))
                    {
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
            Dictionary<string, object> errorResult = GrantHelper.ClassToDict(response);

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, customResponse: errorResult);

            await _events.RaiseAsync(new UserLoginFailureEvent("email", response.Message, false));
    }

    public string  CreateConnectTokenRequest(string userName, string appHash, string grantType,string email)
    {
        var otpToken = oTPService.GenerateOTP(userName, appHash, grantType);

        // URL encoding to make sure tokens are safe for transmission in URL
        string otpEncoded = HttpUtility.UrlEncode(otpToken.Otp);
        string protectTokenEncoded = HttpUtility.UrlEncode(otpToken.ProtectToken);

        string baseUrl = _configuration["VerificationUrl"] ?? "https://localhost:44310"; 

        // Construct the URL with the OTP and Protect Token
        string connectUrl = $"{baseUrl}?UserName={userName}&Email={email}&Otp={otpEncoded}&ProtectToken={protectTokenEncoded}&AppHash={appHash}&GrantType={grantType}";

        otp = otpToken.Otp;
        protectToken = otpToken.ProtectToken;

        return connectUrl;
    }

    
    private async Task<bool> SendOtpAsync(UserIdentity user)
    {
     
        var verificationUrl = CreateConnectTokenRequest(user.UserName, "testhash", GrantType,user.Email);

        var emailBody = emailService.GetMessageBody(new EmailGrantRequest
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        }, verificationUrl);

        // Send the verification email
        var emailSent = await emailService.SendEmailAsync(user.Email, "Email Verification", emailBody);

        if (!emailSent)
        {
            return false;
        }

        return true;
    }


    public async Task<ApiResponse> GenerateForgotPasswordTokenAsync(UserIdentity user)
    {

        // Generate reset password token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        string encodedToken = HttpUtility.UrlEncode(token);

        // Generate the reset password URL
        string baseUrl = _configuration["ResetPasswordUrl"] ?? "https://localhost:44310";
        string resetUrl = $"{baseUrl}?email={HttpUtility.UrlEncode(user.Email)}&token={encodedToken}";

        // Send email with reset password link
        var emailBody = emailService.GetMessageBody(new EmailGrantRequest
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        }, resetUrl);

        bool emailSent = await emailService.SendEmailAsync(user.Email, "Reset Password", emailBody);

        if (!emailSent)
        {
            return new ApiResponse(CustomValMsg.EmailFailedToSend);
        }


        var response = (new ApiResponse("Reset password email sent successfully", null));
        response.IsError = false;

        return response;
    }
}