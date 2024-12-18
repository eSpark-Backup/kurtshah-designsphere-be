using designsphere_identity.STS.Identity.CustomGrants.Models;
using System.Threading.Tasks;

namespace designsphere_identity.STS.Identity.CustomGrants.Services;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string body);

    string GetMessageBody(EmailGrantRequest model, string verificationtoken);

    string GetMessageBodyForOtpRequest(EmailGrantRequest model, string otp);
}
