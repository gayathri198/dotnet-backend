using System.ComponentModel.DataAnnotations;

namespace SkillSwapBackend.ViewModels;

public class LearningSkillViewModel
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SkillName { get; set; } = string.Empty;
}
