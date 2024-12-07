using OtpNet;
using System;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;
using designsphere_identity.STS.Identity.CustomGrants.Configurations;
using designsphere_identity.STS.Identity.CustomGrants.Models;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;
using System.Threading.Tasks;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using designsphere_identity.STS.Identity.CustomGrants.Repository;

namespace designsphere_identity.STS.Identity.CustomGrants.Services;

public interface IOTPService
{
    IOTPToken GenerateOTP(string userName, string apphash, string grantType);

    bool VerifyOTP(string userName, IOTPToken oTPToken, string apphash, string grantType);

    int Validity { get; }
    int Retries { get; }

    Task<ApiResponse> ValidateOTPAsync(string userName);
    Task<ApiResponse> ValidateOTPSentStatusAsync(string userName);
}

public class OTPService : IOTPService
{
    private readonly IOTPConfig oTPConfig;
    private readonly IUserDetailRepository _userDetailRepository;


    public OTPService(IOTPConfig oTPConfig,IUserDetailRepository userDetailRepository)
    {
        this.oTPConfig = oTPConfig;
        _userDetailRepository = userDetailRepository;

    }

    public int Retries
    {
        get { return oTPConfig.Retries; }
    }

    public int Validity
    {
        get { return oTPConfig.Validity; }
    }

    // Convert an object to a byte array
    public bool VerifyOTP(string userName, IOTPToken oTPToken, string apphash, string grantType)
    {
        var bytesOtp = System.Text.Encoding.UTF8.GetBytes($"{oTPConfig.Secret}-{userName}-{apphash}-{grantType}");
        var bytesTOtp = System.Text.Encoding.UTF8.GetBytes($"{oTPConfig.Secret}-{userName}-{apphash}-{oTPToken.Otp}-{grantType}");
        var window = new VerificationWindow(previous: 1, future: 1);
        var tototp = new Totp(bytesOtp, step: oTPConfig.Validity);
        var totpro = new Totp(bytesTOtp, step: oTPConfig.Validity, totpSize: 10);

        var votp = tototp.VerifyTotp(oTPToken.Otp, out long timeStepMatched, window);
        var cotpp = totpro.VerifyTotp(oTPToken.ProtectToken, out long timeStepMatchedp, window);
        return votp && cotpp;
    }

    public IOTPToken GenerateOTP(string userName, string apphash, string grantType)
    {
        var bytesOtp = System.Text.Encoding.UTF8.GetBytes($"{oTPConfig.Secret}-{userName}-{apphash}-{grantType}");
        var totp = new Totp(bytesOtp, step: oTPConfig.Validity);
        var t = DateTime.UtcNow;
        var otp = totp.ComputeTotp(t);

        var bytesTOtp = System.Text.Encoding.UTF8.GetBytes($"{oTPConfig.Secret}-{userName}-{apphash}-{otp}-{grantType}");
        var totp1 = new Totp(bytesTOtp, step: oTPConfig.Validity, totpSize: 10);
        var protectOtp = totp1.ComputeTotp(t);
        return new OTPToken() { Otp = otp, ProtectToken = protectOtp };
    }

   

    public async Task<ApiResponse> ValidateOTPAsync(string userName)
    {
        var userDetail = await _userDetailRepository.GetDetailOfUser(userName);

        if (userDetail == null)
        {
            return new ApiResponse("User details not found.");
        }

        if (userDetail.OTPRetries >= Retries)
        {
            userDetail.OTPRetries++;
            await _userDetailRepository.UpdateAndLog(userDetail);
            return new ApiResponse(CustomValMsg.MaxOtpReached);
        }

        return null; 
    }

    public async Task<ApiResponse> ValidateOTPSentStatusAsync(string userName)
    {
        var userDetail = await _userDetailRepository.GetDetailOfUser(userName);

        if (userDetail == null)
        {
            return new ApiResponse("User details not found.");
        }

        if (userDetail.OTPStatus == EOTPStatus.OTPSent &&
            userDetail.CreationTime <= DateTime.UtcNow.AddSeconds(300))
        {
            userDetail.OTPRetries++;
            await _userDetailRepository.UpdateAndLog(userDetail);
            return new ApiResponse(CustomValMsg.OtpAlreadySent);
        }

        return null; 
    }
}
