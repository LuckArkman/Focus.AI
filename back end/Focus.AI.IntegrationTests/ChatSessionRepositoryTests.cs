using FluentAssertions;
using Focus.AI.Domain.Entities;
using Focus.AI.Infrastructure.Data;
using Focus.AI.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Testcontainers.MongoDb;

namespace Focus.AI.IntegrationTests;

public class ChatSessionRepositoryTests : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:7.0")
        .Build();

    private MongoDbContext _dbContext = null!;
    private ChatSessionRepository _repository = null!;

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();

        var settings = new MongoDbSettings
        {
            ConnectionString = _mongoDbContainer.GetConnectionString(),
            DatabaseName = "test_db",
            ChatSessionsCollection = "chat_sessions"
        };

        var options = Options.Create(settings);
        _dbContext = new MongoDbContext(options);
        _repository = new ChatSessionRepository(_dbContext);
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
    }

    [Fact]
    public async Task CreateSessionAndAddMessages_WorksCorrectly()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        var session = new ChatSession
        {
            Id = sessionId,
            UserId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await _repository.CreateSessionAsync(session);

        var msg1 = new ChatMessage { SessionId = sessionId, Role = "User", Content = "Hello", Timestamp = DateTime.UtcNow.AddMinutes(-5) };
        var msg2 = new ChatMessage { SessionId = sessionId, Role = "Assistant", Content = "Hi there!", Timestamp = DateTime.UtcNow.AddMinutes(-4) };

        await _repository.AddMessageToSessionAsync(sessionId, msg1);
        await _repository.AddMessageToSessionAsync(sessionId, msg2);

        var history = (await _repository.GetSessionHistoryAsync(sessionId, 10)).ToList();

        // Assert
        history.Should().HaveCount(2);
        history[0].Content.Should().Be("Hello");
        history[1].Content.Should().Be("Hi there!");
    }

    [Fact]
    public async Task GetSessionHistoryAsync_LimitsToLastNMessages()
    {
        // Arrange
        var sessionId = Guid.NewGuid().ToString();
        var session = new ChatSession { Id = sessionId, UserId = Guid.NewGuid() };
        await _repository.CreateSessionAsync(session);

        for (int i = 0; i < 5; i++)
        {
            await _repository.AddMessageToSessionAsync(sessionId, new ChatMessage 
            { 
                SessionId = sessionId, 
                Role = "User", 
                Content = $"Msg {i}",
                Timestamp = DateTime.UtcNow.AddSeconds(i)
            });
        }

        // Act
        var history = (await _repository.GetSessionHistoryAsync(sessionId, 2)).ToList();

        // Assert
        history.Should().HaveCount(2);
        history[0].Content.Should().Be("Msg 3");
        history[1].Content.Should().Be("Msg 4");
    }
}
