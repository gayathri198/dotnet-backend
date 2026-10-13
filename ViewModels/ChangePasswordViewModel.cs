using System.ComponentModel.DataAnnotations;

namespace SkillSwapBackend.ViewModels;

public class ChangePasswordViewModel
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    public string NewPassword { get; set; } = string.Empty;
}
