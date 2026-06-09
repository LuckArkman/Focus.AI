using Focus.AI.Domain.Entities;

namespace Focus.AI.Application.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
