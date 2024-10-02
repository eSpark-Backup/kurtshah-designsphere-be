using System.ComponentModel.DataAnnotations;

namespace designsphere_identity.STS.Identity.ViewModels.Manage;

public class SetPasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; }

    public string StatusMessage { get; set; }
}
