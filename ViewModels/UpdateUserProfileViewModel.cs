using System.ComponentModel.DataAnnotations;

namespace SkillSwapBackend.ViewModels;

public class UpdateUserProfileViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string MobileNumber { get; set; } = string.Empty;
}
