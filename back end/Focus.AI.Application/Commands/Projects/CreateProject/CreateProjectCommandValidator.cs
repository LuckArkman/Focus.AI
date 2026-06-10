using FluentValidation;

namespace Focus.AI.Application.Commands.Projects.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(255).WithMessage("Project name cannot exceed 255 characters.");

        RuleFor(x => x.LocalPath).NotEmpty().WithMessage("Local path is required.")
            .MaximumLength(1024).WithMessage("Local path cannot exceed 1024 characters.");
            
        RuleFor(x => x.LanguageStack).MaximumLength(100).WithMessage("Language stack cannot exceed 100 characters.");
    }
}
