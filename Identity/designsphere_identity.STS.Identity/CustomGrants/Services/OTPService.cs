using OtpNet;
using System;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;
using designsphere_identity.STS.Identity.CustomGrants.Configurations;
using designsphere_identity.STS.Identity.CustomGrants.Models;

namespace designsphere_identity.STS.Identity.CustomGrants.Services;

public interface IOTPService
{
    IOTPToken GenerateOTP(string userName, string apphash, string grantType);

    bool VerifyOTP(string userName, IOTPToken oTPToken, string apphash, string grantType);

    int Validity { get; }
    int Retries { get; }
}

public class OTPService : IOTPService
{
    private readonly IOTPConfig oTPConfig;

    public OTPService(IOTPConfig oTPConfig)
    {
        this.oTPConfig = oTPConfig;
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
}
