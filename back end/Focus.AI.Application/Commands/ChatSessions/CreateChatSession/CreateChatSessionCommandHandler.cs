using Focus.AI.Application.Interfaces.Authentication;
using Focus.AI.Domain.Entities;
using Focus.AI.Domain.Interfaces;
using MediatR;

namespace Focus.AI.Application.Commands.ChatSessions.CreateChatSession;

public class CreateChatSessionCommandHandler : IRequestHandler<CreateChatSessionCommand, string>
{
    private readonly IChatSessionRepository _chatSessionRepository;
    private readonly ICurrentUserContext _currentUserContext;

    public CreateChatSessionCommandHandler(IChatSessionRepository chatSessionRepository, ICurrentUserContext currentUserContext)
    {
        _chatSessionRepository = chatSessionRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task<string> Handle(CreateChatSessionCommand request, CancellationToken cancellationToken)
    {
        var session = new ChatSession
        {
            UserId = _currentUserContext.UserId,
            ProjectId = request.ProjectId,
            CreatedAt = DateTime.UtcNow
        };

        var createdSession = await _chatSessionRepository.CreateSessionAsync(session, cancellationToken);
        return createdSession.Id;
    }
}
