using FluentAssertions;
using Focus.AI.Infrastructure.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace Focus.AI.IntegrationTests;

public class LocalEmbeddingGeneratorTests : IAsyncLifetime
{
    private LocalEmbeddingGenerator _generator = null!;

    public async Task InitializeAsync()
    {
        var downloader = new ModelDownloaderService(new NullLogger<ModelDownloaderService>());
        await downloader.StartAsync(CancellationToken.None);
        _generator = new LocalEmbeddingGenerator();
    }

    public Task DisposeAsync()
    {
        _generator?.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GenerateEmbeddingAsync_ReturnsCorrectDimension()
    {
        var embedding = await _generator.GenerateEmbeddingAsync("Test string");
        embedding.Should().HaveCount(384);
    }

    [Fact]
    public async Task GenerateEmbeddingAsync_SimilarSentencesHaveHighCosineSimilarity()
    {
        var emb1 = await _generator.GenerateEmbeddingAsync("Deletar a tabela do postgres");
        var emb2 = await _generator.GenerateEmbeddingAsync("Remover tabela no PostgreSQL");
        var emb3 = await _generator.GenerateEmbeddingAsync("The weather is nice today in Tokyo");

        float sim12 = CosineSimilarity(emb1, emb2);
        float sim13 = CosineSimilarity(emb1, emb3);

        sim12.Should().BeGreaterThan(0.70f); // Depending on model bias, these sentences should be somewhat similar
        sim12.Should().BeGreaterThan(sim13);
        sim13.Should().BeLessThan(0.50f);
    }

    private float CosineSimilarity(float[] vector1, float[] vector2)
    {
        float dotProduct = 0;
        float normA = 0;
        float normB = 0;
        for (int i = 0; i < vector1.Length; i++)
        {
            dotProduct += vector1[i] * vector2[i];
            normA += vector1[i] * vector1[i];
            normB += vector2[i] * vector2[i];
        }
        return dotProduct / ((float)Math.Sqrt(normA) * (float)Math.Sqrt(normB));
    }
}
