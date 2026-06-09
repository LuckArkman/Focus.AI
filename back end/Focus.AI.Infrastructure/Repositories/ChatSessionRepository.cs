using Focus.AI.Domain.Entities;
using Focus.AI.Domain.Interfaces;
using Focus.AI.Infrastructure.Data;
using MongoDB.Driver;

namespace Focus.AI.Infrastructure.Repositories;

public class ChatSessionRepository : IChatSessionRepository
{
    private readonly IMongoCollection<ChatSession> _sessions;

    public ChatSessionRepository(MongoDbContext context)
    {
        _sessions = context.ChatSessions;
    }

    public async Task<ChatSession> CreateSessionAsync(ChatSession session, CancellationToken cancellationToken = default)
    {
        await _sessions.InsertOneAsync(session, cancellationToken: cancellationToken);
        return session;
    }

    public async Task AddMessageToSessionAsync(string sessionId, ChatMessage message, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatSession>.Filter.Eq(s => s.Id, sessionId);
        var update = Builders<ChatSession>.Update.Push(s => s.Messages, message);
        await _sessions.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ChatMessage>> GetSessionHistoryAsync(string sessionId, int lastNMessages, CancellationToken cancellationToken = default)
    {
        var projection = Builders<ChatSession>.Projection.Slice(s => s.Messages, -lastNMessages);
        var session = await _sessions.Find(s => s.Id == sessionId)
            .Project<ChatSession>(projection)
            .FirstOrDefaultAsync(cancellationToken);

        if (session == null || session.Messages == null)
        {
            return Enumerable.Empty<ChatMessage>();
        }

        return session.Messages.OrderBy(m => m.Timestamp);
    }
}
