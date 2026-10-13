using System.ComponentModel.DataAnnotations;

namespace SkillSwapBackend.Models;

public class Message
{
    public int Id { get; set; }

    [Required]
    public int SenderId { get; set; }

    [Required]
    public int ReceiverId { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? Sender { get; set; }

    public User? Receiver { get; set; }
}
