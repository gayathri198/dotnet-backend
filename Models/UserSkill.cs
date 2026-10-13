using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillSwapBackend.Models;

public class UserSkill
{
    [Key]
    public int UserSkillId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SkillName { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Level { get; set; } = "Beginner";

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
