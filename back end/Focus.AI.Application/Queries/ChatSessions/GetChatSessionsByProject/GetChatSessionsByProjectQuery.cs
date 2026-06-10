using MediatR;

namespace Focus.AI.Application.Queries.ChatSessions.GetChatSessionsByProject;

public record GetChatSessionsByProjectQuery(Guid ProjectId) : IRequest<IEnumerable<ChatSessionDto>>;

public class ChatSessionDto
{
    public string Id { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}
