using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using designsphere_identity.STS.Identity.CustomGrants.Configurations;
using designsphere_identity.STS.Identity.CustomGrants.Models;

namespace designsphere_identity.STS.Identity.CustomGrants.Services;

public class SmsService : ISmsService
{
    private readonly ISMSConfig sMSConfig;

    public SmsService(ISMSConfig sMSConfig)
    {
        this.sMSConfig = sMSConfig;
    }

    public string GetMessageBody(PhoneGrantRequest model, string verificationtoken)
    {
        return $"<#>{Environment.NewLine}Your OTP code for designsphere " +
            $"app is {verificationtoken} {Environment.NewLine} For any issues, contact " +
            $"us at phonenumber or info@designsphere.com {Environment.NewLine}{model.AppHash}";
    }

    public async Task<bool> SendAsync(string phoneNumber, string message, bool dryrun)
    {
        using (var client = new HttpClient())
        {
            //dryrun = true; // just for development
            if (dryrun)
                return true;

            var tokenKey = sMSConfig.Secret;
            var sms = HttpUtility.UrlEncode(message);
            client.BaseAddress = new Uri(sMSConfig.ServerAddress);

            var responseTask = await client.GetAsync($"api/sms/send?Token={tokenKey}&number={phoneNumber}&message={sms}");

            if (responseTask.IsSuccessStatusCode)
                return true;
        }

        return false;
    }
}
