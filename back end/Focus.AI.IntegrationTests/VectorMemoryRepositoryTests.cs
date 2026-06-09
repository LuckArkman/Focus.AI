using FluentAssertions;
using Focus.AI.Infrastructure.Repositories;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using Testcontainers.Qdrant;

namespace Focus.AI.IntegrationTests;

public class VectorMemoryRepositoryTests : IAsyncLifetime
{
    private readonly QdrantContainer _qdrantContainer = new QdrantBuilder()
        .WithImage("qdrant/qdrant:v1.9.0")
        .Build();

    private QdrantClient _qdrantClient = null!;
    private VectorMemoryRepository _repository = null!;

    public async Task InitializeAsync()
    {
        await _qdrantContainer.StartAsync();

        _qdrantClient = new QdrantClient("localhost", _qdrantContainer.GetMappedPublicPort(6334));
        
        // Create collection
        await _qdrantClient.CreateCollectionAsync(
            collectionName: "UserInteractions",
            vectorsConfig: new VectorParams { Size = 3, Distance = Distance.Cosine }
        );

        _repository = new VectorMemoryRepository(_qdrantClient);
    }

    public async Task DisposeAsync()
    {
        await _qdrantContainer.DisposeAsync();
    }

    [Fact]
    public async Task UpsertAndSearch_RespectsMultiTenancy()
    {
        // Arrange
        var user1 = Guid.NewGuid();
        var user2 = Guid.NewGuid();

        // Vectors for User 1
        await _repository.UpsertInteractionVectorAsync(user1, "session1", "User 1 Text A", new float[] { 1.0f, 0.0f, 0.0f });
        await _repository.UpsertInteractionVectorAsync(user1, "session1", "User 1 Text B", new float[] { 0.9f, 0.1f, 0.0f });
        
        // Vectors for User 2 (very similar to User 1's A vector)
        await _repository.UpsertInteractionVectorAsync(user2, "session2", "User 2 Text A", new float[] { 1.0f, 0.0f, 0.0f });

        // Act
        // Search as User 1 for the exact same vector
        var results = (await _repository.SearchSimilarContextAsync(user1, new float[] { 1.0f, 0.0f, 0.0f }, 5)).ToList();

        // Assert
        results.Should().NotContain("User 2 Text A", "Multi-tenancy filter failed! Found another user's vector.");
        results.Should().Contain("User 1 Text A");
        results.Should().Contain("User 1 Text B");
    }
}
