using Focus.AI.Api.Extensions;
using Focus.AI.Application.Interfaces.Authentication;

namespace Focus.AI.Api.Services;

public class CurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                throw new UnauthorizedAccessException("HttpContext User is null.");
            }
            return user.GetUserId();
        }
    }
}
