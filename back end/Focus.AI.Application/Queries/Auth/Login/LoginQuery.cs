using MediatR;

namespace Focus.AI.Application.Queries.Auth.Login;

public class LoginQuery : IRequest<string>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
