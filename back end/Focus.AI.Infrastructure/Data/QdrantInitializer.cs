using Microsoft.Extensions.Hosting;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace Focus.AI.Infrastructure.Data;

public class QdrantInitializer : IHostedService
{
    private readonly QdrantClient _qdrantClient;

    public QdrantInitializer(QdrantClient qdrantClient)
    {
        _qdrantClient = qdrantClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var collectionName = "UserInteractions";
        var collections = await _qdrantClient.ListCollectionsAsync(cancellationToken: cancellationToken);

        if (!collections.Contains(collectionName))
        {
            await _qdrantClient.CreateCollectionAsync(
                collectionName: collectionName,
                vectorsConfig: new VectorParams { Size = 384, Distance = Distance.Cosine },
                cancellationToken: cancellationToken
            );
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
