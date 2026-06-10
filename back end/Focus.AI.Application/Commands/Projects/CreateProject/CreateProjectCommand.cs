using MediatR;

namespace Focus.AI.Application.Commands.Projects.CreateProject;

public record CreateProjectCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
    public string LocalPath { get; init; } = string.Empty;
    public string LanguageStack { get; init; } = string.Empty;
}
