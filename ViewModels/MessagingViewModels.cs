using System.ComponentModel.DataAnnotations;

namespace SkillSwapBackend.ViewModels;

public class SendMessageViewModel
{
    [Range(1, int.MaxValue)]
    public int ReceiverId { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
}

public class MessageViewModel
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class ConversationViewModel
{
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public MessageViewModel LastMessage { get; set; } = new();
}
