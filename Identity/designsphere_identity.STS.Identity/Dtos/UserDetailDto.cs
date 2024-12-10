using Microsoft.AspNetCore.Http;

namespace designsphere_identity.STS.Identity.Dtos;

public class UserDetailDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNo { get; set; }

    public string CompanyName { get; set; }
    public string Country { get; set; }
    public string JobFunction { get; set; }
    public string PostalCode { get; set; }

    public bool IsEmailMFA { get; set; }
    public bool IsGoogleMFA { get; set; }
    public UserPicture ProfilePicture { get; set; }
}

public class UserPicture
{
    public string FileName { get; set; }

    public byte[] bytes { get; set; }

    public IFormFile File { get; set; }
}