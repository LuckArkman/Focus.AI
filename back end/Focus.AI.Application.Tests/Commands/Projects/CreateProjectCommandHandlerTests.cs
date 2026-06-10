using FluentAssertions;
using Focus.AI.Application.Commands.Projects.CreateProject;
using Focus.AI.Application.Exceptions;
using Focus.AI.Application.Interfaces.Authentication;
using Focus.AI.Domain.Entities;
using Focus.AI.Domain.Interfaces;
using Moq;
using Xunit;

namespace Focus.AI.Application.Tests.Commands.Projects;

public class CreateProjectCommandHandlerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<ICurrentUserContext> _currentUserContextMock;
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _currentUserContextMock = new Mock<ICurrentUserContext>();
        _handler = new CreateProjectCommandHandler(_projectRepositoryMock.Object, _currentUserContextMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProjectDoesNotExist_ShouldAddProjectAndReturnId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserContextMock.Setup(x => x.UserId).Returns(userId);

        var command = new CreateProjectCommand
        {
            Name = "focus.ai",
            LocalPath = "i:\\Focus.AI",
            LanguageStack = "C#"
        };

        _projectRepositoryMock.Setup(x => x.GetProjectByLocalPathAsync(userId, command.LocalPath, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _projectRepositoryMock.Verify(x => x.AddProjectAsync(It.Is<Project>(p => 
            p.Name == command.Name && 
            p.LocalPath == command.LocalPath && 
            p.LanguageStack == command.LanguageStack &&
            p.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProjectAlreadyExists_ShouldThrowProjectAlreadyExistsException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserContextMock.Setup(x => x.UserId).Returns(userId);

        var command = new CreateProjectCommand
        {
            Name = "focus.ai",
            LocalPath = "i:\\Focus.AI",
            LanguageStack = "C#"
        };

        _projectRepositoryMock.Setup(x => x.GetProjectByLocalPathAsync(userId, command.LocalPath, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Project { Id = Guid.NewGuid() });

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ProjectAlreadyExistsException>()
            .WithMessage($"A project mapping for the path '{command.LocalPath}' already exists for this user.");
        
        _projectRepositoryMock.Verify(x => x.AddProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
