using MediatR;

namespace Focus.AI.Application.Queries.ChatSessions.GetMessagesBySession;

public record GetMessagesBySessionQuery(string SessionId, int Take = 50) : IRequest<IEnumerable<ChatMessageDto>>;

public class ChatMessageDto
{
    public string Id { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
