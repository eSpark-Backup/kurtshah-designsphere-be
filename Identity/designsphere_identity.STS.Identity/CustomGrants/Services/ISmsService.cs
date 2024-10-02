using System.Threading.Tasks;
using designsphere_identity.STS.Identity.CustomGrants.Models;

namespace designsphere_identity.STS.Identity.CustomGrants.Services;

public interface ISmsService
{
    Task<bool> SendAsync(string phoneNumber, string message, bool dryrun);

    string GetMessageBody(PhoneGrantRequest model, string verificationtoken);
}
