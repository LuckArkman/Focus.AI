using Focus.AI.Domain.Entities;

namespace Focus.AI.Domain.Interfaces;

public interface IProjectRepository
{
    Task AddProjectAsync(Project project, CancellationToken cancellationToken = default);
    Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Project?> GetProjectByLocalPathAsync(Guid userId, string localPath, CancellationToken cancellationToken = default);
}
