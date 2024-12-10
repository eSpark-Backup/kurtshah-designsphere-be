using System.ComponentModel.DataAnnotations;

namespace designsphere_identity.STS.Identity.CustomGrants.Models;

public class ResetPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }
}
