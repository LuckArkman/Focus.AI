using Focus.AI.Application.Interfaces.Authentication;
using Focus.AI.Domain.Interfaces;
using MediatR;

namespace Focus.AI.Application.Queries.Projects.GetProjectsByUser;

public class GetProjectsByUserQueryHandler : IRequestHandler<GetProjectsByUserQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserContext _currentUserContext;

    public GetProjectsByUserQueryHandler(IProjectRepository projectRepository, ICurrentUserContext currentUserContext)
    {
        _projectRepository = projectRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsByUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserContext.UserId;
        var projects = await _projectRepository.GetProjectsByUserIdAsync(userId, cancellationToken);

        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            LocalPath = p.LocalPath,
            LanguageStack = p.LanguageStack,
            CreatedAt = p.CreatedAt
        });
    }
}
