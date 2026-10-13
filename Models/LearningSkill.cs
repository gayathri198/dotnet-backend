
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillSwapBackend.Models;

public class LearningSkill
{
    [Key]
    public int LearningSkillId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SkillName { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
