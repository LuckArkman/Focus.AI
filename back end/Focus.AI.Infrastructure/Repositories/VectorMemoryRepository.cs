using Focus.AI.Application.Interfaces;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace Focus.AI.Infrastructure.Repositories;

public class VectorMemoryRepository : IVectorMemoryRepository
{
    private readonly QdrantClient _qdrantClient;
    private const string CollectionName = "UserInteractions";

    public VectorMemoryRepository(QdrantClient qdrantClient)
    {
        _qdrantClient = qdrantClient;
    }

    public async Task UpsertInteractionVectorAsync(Guid userId, string sessionId, string rawText, float[] vector)
    {
        var qdrantVector = new Vector();
        qdrantVector.Data.AddRange(vector);

        var point = new PointStruct
        {
            Id = Guid.NewGuid(),
            Vectors = new Vectors { Vector = qdrantVector },
            Payload =
            {
                ["user_id"] = userId.ToString(),
                ["session_id"] = sessionId,
                ["text"] = rawText
            }
        };

        await _qdrantClient.UpsertAsync(CollectionName, new[] { point });
    }

    public async Task<IEnumerable<string>> SearchSimilarContextAsync(Guid userId, float[] queryVector, int limit = 5)
    {
        // STRICT payload filter for multi-tenancy
        var filter = new Filter();
        filter.Must.Add(new Condition
        {
            Field = new FieldCondition
            {
                Key = "user_id",
                Match = new Match { Keyword = userId.ToString() }
            }
        });

        var searchResults = await _qdrantClient.SearchAsync(
            collectionName: CollectionName,
            vector: queryVector,
            filter: filter,
            limit: (ulong)limit
        );

        return searchResults.Select(r => r.Payload["text"].StringValue);
    }
}
