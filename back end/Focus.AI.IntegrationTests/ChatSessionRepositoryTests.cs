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
        var session = new ChatSession
        {
            UserId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await _repository.CreateSessionAsync(session);
        var sessionId = session.Id;

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
        var session = new ChatSession { UserId = Guid.NewGuid() };
        await _repository.CreateSessionAsync(session);
        var sessionId = session.Id;

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

    [Fact]
    public async Task GetSessionsByProjectIdAsync_ReturnsSessionsOrderedByCreatedAtDesc()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var session1 = new ChatSession { UserId = Guid.NewGuid(), ProjectId = projectId, CreatedAt = DateTime.UtcNow.AddMinutes(-10) };
        var session2 = new ChatSession { UserId = Guid.NewGuid(), ProjectId = projectId, CreatedAt = DateTime.UtcNow.AddMinutes(-5) };
        var sessionOther = new ChatSession { UserId = Guid.NewGuid(), ProjectId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };

        await _repository.CreateSessionAsync(session1);
        await _repository.CreateSessionAsync(session2);
        await _repository.CreateSessionAsync(sessionOther);

        // Act
        var sessions = (await _repository.GetSessionsByProjectIdAsync(projectId)).ToList();

        // Assert
        sessions.Should().HaveCount(2);
        sessions[0].Id.Should().Be(session2.Id); // session2 is newer
        sessions[1].Id.Should().Be(session1.Id);
    }

    [Fact]
    public async Task GetSessionHistoryAsync_UnderLoad_ReturnsQuickly()
    {
        // Arrange
        var session = new ChatSession { UserId = Guid.NewGuid() };
        await _repository.CreateSessionAsync(session);
        var sessionId = session.Id;

        var messages = Enumerable.Range(0, 100).Select(i => new ChatMessage
        {
            SessionId = sessionId,
            Role = "User",
            Content = $"Message {i}",
            Timestamp = DateTime.UtcNow.AddSeconds(i)
        }).ToList();

        foreach (var msg in messages)
        {
            await _repository.AddMessageToSessionAsync(sessionId, msg);
        }

        // Act
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var history = (await _repository.GetSessionHistoryAsync(sessionId, 50)).ToList();
        watch.Stop();

        // Assert
        history.Should().HaveCount(50);
        watch.ElapsedMilliseconds.Should().BeLessThan(200); // Allow slightly higher threshold for tests running in containers
    }
}
