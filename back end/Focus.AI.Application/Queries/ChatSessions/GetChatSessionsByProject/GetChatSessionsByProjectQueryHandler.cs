using Focus.AI.Domain.Interfaces;
using MediatR;

namespace Focus.AI.Application.Queries.ChatSessions.GetChatSessionsByProject;

public class GetChatSessionsByProjectQueryHandler : IRequestHandler<GetChatSessionsByProjectQuery, IEnumerable<ChatSessionDto>>
{
    private readonly IChatSessionRepository _chatSessionRepository;

    public GetChatSessionsByProjectQueryHandler(IChatSessionRepository chatSessionRepository)
    {
        _chatSessionRepository = chatSessionRepository;
    }

    public async Task<IEnumerable<ChatSessionDto>> Handle(GetChatSessionsByProjectQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _chatSessionRepository.GetSessionsByProjectIdAsync(request.ProjectId, cancellationToken);
        
        return sessions.Select(s => new ChatSessionDto
        {
            Id = s.Id,
            ProjectId = s.ProjectId ?? Guid.Empty,
            CreatedAt = s.CreatedAt
        });
    }
}
