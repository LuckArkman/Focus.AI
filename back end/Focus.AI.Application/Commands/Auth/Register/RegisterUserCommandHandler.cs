using BCrypt.Net;
using Focus.AI.Domain.Entities;
using Focus.AI.Domain.Interfaces;
using MediatR;

namespace Focus.AI.Application.Commands.Auth.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new Exception("Email already exists.");
        }

        var role = await _roleRepository.GetByNameAsync("User", cancellationToken);
        if (role == null)
        {
            throw new Exception("Default 'User' role not found in the database.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            RoleId = role.Id
        };

        await _userRepository.AddAsync(user, cancellationToken);
        return user.Id.ToString();
    }
}
