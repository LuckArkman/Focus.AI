using MediatR;

namespace Focus.AI.Application.Commands.ChatSessions.CreateChatSession;

public record CreateChatSessionCommand(Guid ProjectId) : IRequest<string>;
