using FluentAssertions;
using Focus.AI.Application.EventHandlers;
using Focus.AI.Application.Events;
using Focus.AI.Application.Interfaces;
using Focus.AI.Application.Interfaces.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Focus.AI.Application.Tests.EventHandlers;

public class PersistVectorMemoryHandlerTests
{
    [Fact]
    public async Task Handle_GeneratesEmbeddingAndUpsertsToVectorDatabase()
    {
        // Arrange
        var embeddingGeneratorMock = new Mock<IEmbeddingGenerator>();
        var vectorMemoryRepoMock = new Mock<IVectorMemoryRepository>();

        var testVector = new float[384];
        for(int i = 0; i < 384; i++) testVector[i] = 1.0f;

        embeddingGeneratorMock
            .Setup(x => x.GenerateEmbeddingAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testVector);

        var handler = new PersistVectorMemoryHandler(
            embeddingGeneratorMock.Object,
            vectorMemoryRepoMock.Object,
            new NullLogger<PersistVectorMemoryHandler>());

        var notification = new MessageAddedEvent(
            UserId: Guid.NewGuid(),
            SessionId: "session-123",
            MessageId: "msg-456",
            Content: "Hello World"
        );

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        embeddingGeneratorMock.Verify(x => x.GenerateEmbeddingAsync("Hello World", It.IsAny<CancellationToken>()), Times.Once);
        vectorMemoryRepoMock.Verify(x => x.UpsertInteractionVectorAsync(notification.UserId, "session-123", "Hello World", testVector), Times.Once);
    }
}
