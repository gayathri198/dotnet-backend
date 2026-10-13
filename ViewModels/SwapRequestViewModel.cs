using System.ComponentModel.DataAnnotations;

namespace SkillSwapBackend.ViewModels;

public class CreateSwapRequestViewModel
{
    [Range(1, int.MaxValue)]
    public int SenderId { get; set; }

    [Range(1, int.MaxValue)]
    public int ReceiverId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SkillOffered { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string SkillWanted { get; set; } = string.Empty;
}

public class UpdateSwapRequestStatusViewModel
{
    [Required]
    public string Status { get; set; } = string.Empty;
}

public class SwapRequestViewModel
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public string SenderName { get; set; } = string.Empty;

    public int ReceiverId { get; set; }

    public string ReceiverName { get; set; } = string.Empty;

    public string SkillOffered { get; set; } = string.Empty;

    public string SkillWanted { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
