using Focus.AI.Domain.Entities;
using Focus.AI.Domain.Interfaces;
using Focus.AI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Focus.AI.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddProjectAsync(Project project, CancellationToken cancellationToken = default)
    {
        await _context.Projects.AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Project?> GetProjectByLocalPathAsync(Guid userId, string localPath, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.UserId == userId && p.LocalPath == localPath, cancellationToken);
    }

    public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .Where(p => p.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}
