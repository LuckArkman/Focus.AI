using MediatR;

namespace Focus.AI.Application.Commands.ChatSessions.AddMessage;

public record AddMessageCommand(string SessionId, string Role, string Content, int TokensUsed = 0) : IRequest;
