using System.ComponentModel.DataAnnotations;

namespace designsphere_identity.STS.Identity.CustomGrants.Models;

public class DeleteAccountRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
