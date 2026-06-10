using Focus.AI.Application.Events;
using Focus.AI.Application.Interfaces.Authentication;
using Focus.AI.Domain.Entities;
using Focus.AI.Domain.Interfaces;
using MediatR;

namespace Focus.AI.Application.Commands.ChatSessions.AddMessage;

public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand>
{
    private readonly IChatSessionRepository _chatSessionRepository;
    private readonly IPublisher _publisher;
    private readonly ICurrentUserContext _currentUserContext;

    public AddMessageCommandHandler(IChatSessionRepository chatSessionRepository, IPublisher publisher, ICurrentUserContext currentUserContext)
    {
        _chatSessionRepository = chatSessionRepository;
        _publisher = publisher;
        _currentUserContext = currentUserContext;
    }

    public async Task Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new ChatMessage
        {
            Id = Guid.NewGuid().ToString(),
            SessionId = request.SessionId,
            Role = request.Role,
            Content = request.Content,
            TokensUsed = request.TokensUsed,
            Timestamp = DateTime.UtcNow
        };

        await _chatSessionRepository.AddMessageToSessionAsync(request.SessionId, message, cancellationToken);

        await _publisher.Publish(new MessageAddedEvent(_currentUserContext.UserId, request.SessionId, message.Id, message.Content), cancellationToken);
    }
}
