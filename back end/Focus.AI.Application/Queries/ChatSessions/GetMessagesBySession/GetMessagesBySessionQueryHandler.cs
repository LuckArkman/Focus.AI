using Focus.AI.Domain.Interfaces;
using MediatR;

namespace Focus.AI.Application.Queries.ChatSessions.GetMessagesBySession;

public class GetMessagesBySessionQueryHandler : IRequestHandler<GetMessagesBySessionQuery, IEnumerable<ChatMessageDto>>
{
    private readonly IChatSessionRepository _chatSessionRepository;

    public GetMessagesBySessionQueryHandler(IChatSessionRepository chatSessionRepository)
    {
        _chatSessionRepository = chatSessionRepository;
    }

    public async Task<IEnumerable<ChatMessageDto>> Handle(GetMessagesBySessionQuery request, CancellationToken cancellationToken)
    {
        var messages = await _chatSessionRepository.GetSessionHistoryAsync(request.SessionId, request.Take, cancellationToken);

        return messages.Select(m => new ChatMessageDto
        {
            Id = m.Id,
            Role = m.Role,
            Content = m.Content,
            Timestamp = m.Timestamp
        });
    }
}
