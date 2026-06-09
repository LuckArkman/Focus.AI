namespace Focus.AI.Application.Interfaces;

public interface IVectorMemoryRepository
{
    Task UpsertInteractionVectorAsync(Guid userId, string sessionId, string rawText, float[] vector);
    Task<IEnumerable<string>> SearchSimilarContextAsync(Guid userId, float[] queryVector, int limit = 5);
}
