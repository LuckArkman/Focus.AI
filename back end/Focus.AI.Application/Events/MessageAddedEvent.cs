using MediatR;

namespace Focus.AI.Application.Events;

public record MessageAddedEvent(Guid UserId, string SessionId, string MessageId, string Content) : INotification;
