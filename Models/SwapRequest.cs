using System.ComponentModel.DataAnnotations;

namespace SkillSwapBackend.Models;

public class SwapRequest
{
    public int Id { get; set; }

    [Required]
    public int SenderId { get; set; }

    [Required]
    public int ReceiverId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SkillOffered { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string SkillWanted { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? Sender { get; set; }

    public User? Receiver { get; set; }
}
