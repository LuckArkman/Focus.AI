using MediatR;

namespace Focus.AI.Application.Commands.Auth.Register;

public class RegisterUserCommand : IRequest<string>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
