using Focus.AI.Application.Interfaces.Authentication;
using Focus.AI.Domain.Interfaces;
using MediatR;

namespace Focus.AI.Application.Queries.Projects.GetProjectStatus;

public class GetProjectStatusQueryHandler : IRequestHandler<GetProjectStatusQuery, ProjectStatusDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserContext _currentUserContext;

    public GetProjectStatusQueryHandler(IProjectRepository projectRepository, ICurrentUserContext currentUserContext)
    {
        _projectRepository = projectRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task<ProjectStatusDto> Handle(GetProjectStatusQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserContext.UserId;
        
        // No futuro, validaremos também se o projeto pertence ao usuário e checaremos no Qdrant
        // Por hora retornaremos o status mockado para atender à Sprint 12 inicial.
        
        return await Task.FromResult(new ProjectStatusDto
        {
            ProjectId = request.ProjectId,
            IsIndexed = true, // Dummy value
            Status = "Ready"  // Dummy value
        });
    }
}
