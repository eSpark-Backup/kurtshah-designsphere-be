using System.ComponentModel.DataAnnotations;

namespace designsphere_identity.STS.Identity.ViewModels.Account;

public class LoginWithRecoveryCodeViewModel
{
    [Required]
    [DataType(DataType.Text)]
    public string RecoveryCode { get; set; }

    public string ReturnUrl { get; set; }
}
