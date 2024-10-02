using System.ComponentModel.DataAnnotations;

namespace designsphere_identity.STS.Identity.ViewModels.Account;

public class ExternalLoginConfirmationViewModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9_@\-\.\+]+$")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
