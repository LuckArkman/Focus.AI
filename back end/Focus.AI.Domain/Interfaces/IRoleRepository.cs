using Focus.AI.Domain.Entities;

namespace Focus.AI.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
