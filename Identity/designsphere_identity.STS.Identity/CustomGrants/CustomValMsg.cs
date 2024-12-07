using System;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;

namespace designsphere_identity.STS.Identity.CustomGrants;

public static class CustomValMsg
{
    public const string ParamMissing = "Parameters missing";
    public const string DeviceBlocked = "Device is Blocked, Please Contact to application support";
    public const string MaxOtpReached = "Max OTP Retries Reached, Please Contact to application support";
    public const string OtpAlreadySent = "Otp already sent to Phone number, Please wait for 5 minutes";
    public const string ParamOtpMissed = "Parameters [otptype] missing";
    public const string MessageServiceUnavailable = "Message Service Unavailable";
    public const string GenerateOtpFirst = "Generate otp first...";
    public const string ParamOtpPTokenMissing = "Parameters [Otp, protecttoken] missing";
    public const string OtpExipredOrInvalid = "OTP is expired or invalid";
    public const string InvalidRequest = "Invalid RequestType";
    public const string InvalidAudience = "Invalid audience";
    public const string InvalidIdToken = "Invalid IdToken";
    public const string InvalidEmailPassword = "Invalid email or password";
    public const string UserLockedOut = "User is locked out";
    public const string UserNotExist = "User does not exist";
    public const string UserExist = "User already exist with provided email";
    public const string UserNotVerified = "User not yet verified ";
    public const string EmailNotCorrect = "Entered Email is not correct ";
    public const string EmailFailedToSend = "Email failed to deliver";
    public const string PasswordResetFailed = "Password Reset Failed";

    public static string RoleNotFound(string role)
    {
        return $"{role} role not found";
    }

    public static string OtpSent(UserIdentity user, int validity)
    {
        return $"OTP Sent to {user.PhoneNumber}, valid for {validity} seconds";
    }

    public static string InvalidOtpType()
    {
        return $"Invalid otptype value, accepted value {string.Join(",", Enum.GetNames(typeof(EPhoneOtp)))}";
    }

    public static string DuplicatePhonenumber(UserIdentity user, string phonenumber)
    {
        return $"Phone Number {phonenumber} already registered using {user.Type}.";
    }

    public static string DuplicatePhonenumber(UserIdentity user)
    {
        return $"Phone Number {user.PhoneNumber} already registered using {user.Type}";
    }

    public static string UserLockout(UserIdentity user)
    {
        return $"User is locked until {user.LockoutEnd}";
    }
}
