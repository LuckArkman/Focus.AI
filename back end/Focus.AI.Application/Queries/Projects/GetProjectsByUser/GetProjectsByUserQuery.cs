using Focus.AI.Domain.Entities;
using MediatR;

namespace Focus.AI.Application.Queries.Projects.GetProjectsByUser;

public record GetProjectsByUserQuery : IRequest<IEnumerable<ProjectDto>>;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LocalPath { get; set; } = string.Empty;
    public string LanguageStack { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
