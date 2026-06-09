using MediatR;

namespace Focus.AI.Application.Commands.Ping;

public class PingCommand : IRequest<string>
{
    public string Message { get; set; } = string.Empty;
}
