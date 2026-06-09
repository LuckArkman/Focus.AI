using Focus.AI.Domain.Entities;

namespace Focus.AI.Domain.Interfaces;

public interface IChatSessionRepository
{
    Task<ChatSession> CreateSessionAsync(ChatSession session, CancellationToken cancellationToken = default);
    Task AddMessageToSessionAsync(string sessionId, ChatMessage message, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChatMessage>> GetSessionHistoryAsync(string sessionId, int lastNMessages, CancellationToken cancellationToken = default);
}
