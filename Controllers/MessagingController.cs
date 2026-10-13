using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSwapBackend.Models;
using SkillSwapBackend.Repositories;
using SkillSwapBackend.ViewModels;

namespace SkillSwapBackend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MessagingController : ControllerBase
{
    private const int DefaultPageSize = 50;
    private const int MaximumPageSize = 100;

    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public MessagingController(
        IMessageRepository messageRepository,
        IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    [HttpPost("messages")]
    public async Task<IActionResult> SendMessage(SendMessageViewModel model)
    {
        var senderId = GetCurrentUserId();
        if (senderId == null)
        {
            return Unauthorized();
        }

        if (senderId == model.ReceiverId)
        {
            return BadRequest("A user cannot send a message to themselves.");
        }

        if (string.IsNullOrWhiteSpace(model.Content))
        {
            return BadRequest("Message content cannot be empty.");
        }

        if (await _userRepository.GetUserById(model.ReceiverId) == null)
        {
            return NotFound("Recipient not found.");
        }

        var message = await _messageRepository.CreateAsync(new Message
        {
            SenderId = senderId.Value,
            ReceiverId = model.ReceiverId,
            Content = model.Content.Trim(),
            CreatedAt = DateTime.UtcNow
        });

        return Ok(ToViewModel(message));
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var conversations = await _messageRepository.GetConversationsAsync(userId.Value);
        return Ok(conversations.Select(conversation => new ConversationViewModel
        {
            UserId = conversation.Partner.Id,
            FullName = conversation.Partner.FullName,
            LastMessage = ToViewModel(conversation.LastMessage)
        }));
    }

    [HttpGet("conversations/{otherUserId:int}/messages")]
    public async Task<IActionResult> GetMessages(
        int otherUserId,
        [FromQuery] int? beforeMessageId,
        [FromQuery] int pageSize = DefaultPageSize)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        if (userId == otherUserId)
        {
            return BadRequest("A user cannot start a conversation with themselves.");
        }

        if (await _userRepository.GetUserById(otherUserId) == null)
        {
            return NotFound("User not found.");
        }

        if (beforeMessageId <= 0)
        {
            return BadRequest("beforeMessageId must be greater than zero.");
        }

        var messages = await _messageRepository.GetConversationMessagesAsync(
            userId.Value,
            otherUserId,
            beforeMessageId,
            Math.Clamp(pageSize, 1, MaximumPageSize));

        messages.Reverse();
        return Ok(messages.Select(ToViewModel));
    }

    [HttpGet("conversations/{otherUserId:int}/messages/after")]
    public async Task<IActionResult> GetMessagesAfter(
        int otherUserId,
        [FromQuery] int afterMessageId = 0,
        [FromQuery] int pageSize = DefaultPageSize)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        if (userId == otherUserId)
        {
            return BadRequest("A user cannot start a conversation with themselves.");
        }

        if (await _userRepository.GetUserById(otherUserId) == null)
        {
            return NotFound("User not found.");
        }

        if (afterMessageId < 0)
        {
            return BadRequest("afterMessageId cannot be negative.");
        }

        var messages = await _messageRepository.GetMessagesAfterAsync(
            userId.Value,
            otherUserId,
            afterMessageId,
            Math.Clamp(pageSize, 1, MaximumPageSize));

        return Ok(messages.Select(ToViewModel));
    }

    private int? GetCurrentUserId()
    {
        var subject = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return int.TryParse(subject, out var userId) && userId > 0
            ? userId
            : null;
    }

    private static MessageViewModel ToViewModel(Message message)
    {
        return new MessageViewModel
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ReceiverId = message.ReceiverId,
            Content = message.Content,
            CreatedAt = message.CreatedAt
        };
    }
}
