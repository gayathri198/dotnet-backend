using Microsoft.EntityFrameworkCore;
using SkillSwapBackend.Data;
using SkillSwapBackend.Models;

namespace SkillSwapBackend.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Message> CreateAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<(User Partner, Message LastMessage)>> GetConversationsAsync(int userId)
    {
        var lastMessageIds = await _context.Messages
            .AsNoTracking()
            .Where(message => message.SenderId == userId || message.ReceiverId == userId)
            .GroupBy(message =>
                message.SenderId == userId ? message.ReceiverId : message.SenderId)
            .Select(group => group.Max(message => message.Id))
            .ToListAsync();

        var lastMessages = await _context.Messages
            .AsNoTracking()
            .Where(message => lastMessageIds.Contains(message.Id))
            .OrderByDescending(message => message.Id)
            .ToListAsync();

        var partnerIds = lastMessages
            .Select(message => message.SenderId == userId
                ? message.ReceiverId
                : message.SenderId)
            .Distinct()
            .ToList();

        var partners = await _context.Users
            .AsNoTracking()
            .Where(user => partnerIds.Contains(user.Id))
            .ToDictionaryAsync(user => user.Id);

        return lastMessages
            .Select(message =>
            {
                var partnerId = message.SenderId == userId
                    ? message.ReceiverId
                    : message.SenderId;
                return (partners[partnerId], message);
            })
            .ToList();
    }

    public Task<List<Message>> GetConversationMessagesAsync(
        int userId,
        int otherUserId,
        int? beforeMessageId,
        int pageSize)
    {
        return _context.Messages
            .AsNoTracking()
            .Where(message =>
                (message.SenderId == userId && message.ReceiverId == otherUserId)
                || (message.SenderId == otherUserId && message.ReceiverId == userId))
            .Where(message => beforeMessageId == null || message.Id < beforeMessageId)
            .OrderByDescending(message => message.Id)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<List<Message>> GetMessagesAfterAsync(
        int userId,
        int otherUserId,
        int afterMessageId,
        int pageSize)
    {
        return _context.Messages
            .AsNoTracking()
            .Where(message =>
                (message.SenderId == userId && message.ReceiverId == otherUserId)
                || (message.SenderId == otherUserId && message.ReceiverId == userId))
            .Where(message => message.Id > afterMessageId)
            .OrderBy(message => message.Id)
            .Take(pageSize)
            .ToListAsync();
    }
}
