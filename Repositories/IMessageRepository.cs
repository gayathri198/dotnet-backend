using SkillSwapBackend.Models;

namespace SkillSwapBackend.Repositories;

public interface IMessageRepository
{
    Task<Message> CreateAsync(Message message);

    Task<List<(User Partner, Message LastMessage)>> GetConversationsAsync(int userId);

    Task<List<Message>> GetConversationMessagesAsync(
        int userId,
        int otherUserId,
        int? beforeMessageId,
        int pageSize);

    Task<List<Message>> GetMessagesAfterAsync(
        int userId,
        int otherUserId,
        int afterMessageId,
        int pageSize);
}
