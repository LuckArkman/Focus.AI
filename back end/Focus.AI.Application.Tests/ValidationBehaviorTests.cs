using FluentAssertions;
using FluentValidation;
using Focus.AI.Application.Behaviors;
using MediatR;

namespace Focus.AI.Application.Tests;

public class ValidationBehaviorTests
{
    public class TestCommand : IRequest<string>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class TestCommandValidator : AbstractValidator<TestCommand>
    {
        public TestCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        }
    }

    [Fact]
    public async Task Handle_WithInvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var validators = new List<IValidator<TestCommand>> { new TestCommandValidator() };
        var behavior = new ValidationBehavior<TestCommand, string>(validators);
        var command = new TestCommand { Name = "" };

        // Act
        Func<Task> action = async () => await behavior.Handle(command, delegate { return Task.FromResult("Success"); }, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Name is required*");
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsNext()
    {
        // Arrange
        var validators = new List<IValidator<TestCommand>> { new TestCommandValidator() };
        var behavior = new ValidationBehavior<TestCommand, string>(validators);
        var command = new TestCommand { Name = "Valid" };

        // Act
        var result = await behavior.Handle(command, delegate { return Task.FromResult("Success"); }, CancellationToken.None);

        // Assert
        result.Should().Be("Success");
    }
}
