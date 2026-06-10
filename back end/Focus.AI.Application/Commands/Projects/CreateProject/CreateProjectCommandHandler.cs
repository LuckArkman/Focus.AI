using Focus.AI.Application.Exceptions;
using Focus.AI.Application.Interfaces.Authentication;
using Focus.AI.Domain.Entities;
using Focus.AI.Domain.Interfaces;
using MediatR;

namespace Focus.AI.Application.Commands.Projects.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserContext _currentUserContext;

    public CreateProjectCommandHandler(IProjectRepository projectRepository, ICurrentUserContext currentUserContext)
    {
        _projectRepository = projectRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserContext.UserId;

        var existingProject = await _projectRepository.GetProjectByLocalPathAsync(userId, request.LocalPath, cancellationToken);
        if (existingProject != null)
        {
            throw new ProjectAlreadyExistsException(request.LocalPath);
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            LocalPath = request.LocalPath,
            LanguageStack = request.LanguageStack,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _projectRepository.AddProjectAsync(project, cancellationToken);

        return project.Id;
    }
}
