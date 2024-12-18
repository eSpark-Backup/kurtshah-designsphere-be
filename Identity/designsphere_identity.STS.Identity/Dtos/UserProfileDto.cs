using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace designsphere_identity.STS.Identity.Dtos;

public class UserProfileDto 
{
    public string PostalCode { get; set; } = string.Empty;
    public Guid? CountryId { get; set; }
    public Guid? JobRoleId { get; set; }
    public bool? IsPrivacyPolicyAccepted { get; set; }

    public bool? NewsLetterSubscription { get; set; }
    public string CompanyName { get; set; }

    public IFormFile? ProfilePicture { get; set; }

    public bool IsEmailMFA { get; set; } = false;
    public bool IsGoogleMFA { get; set; } = false;
}
