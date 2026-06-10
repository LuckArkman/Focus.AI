using MediatR;

namespace Focus.AI.Application.Queries.Projects.GetProjectStatus;

public record GetProjectStatusQuery(Guid ProjectId) : IRequest<ProjectStatusDto>;

public class ProjectStatusDto
{
    public Guid ProjectId { get; set; }
    public bool IsIndexed { get; set; }
    public string Status { get; set; } = string.Empty;
}
