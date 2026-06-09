using MediatR;

namespace Focus.AI.Application.Commands.Ping;

public class PingCommandHandler : IRequestHandler<PingCommand, string>
{
    public Task<string> Handle(PingCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Pong: {request.Message}");
    }
}
