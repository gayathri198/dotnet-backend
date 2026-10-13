
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillSwapBackend.Models;

public class Skill
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public string SkillName { get; set; } = string.Empty;

    [Required]
    public string SkillType { get; set; } = string.Empty;

    [Required]
    public string SkillLevel { get; set; } = string.Empty;

    public User? User { get; set; }
}
