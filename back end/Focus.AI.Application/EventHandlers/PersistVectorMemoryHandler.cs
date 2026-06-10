using Focus.AI.Application.Events;
using Focus.AI.Application.Interfaces;
using Focus.AI.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.AI.Application.EventHandlers;

public class PersistVectorMemoryHandler : INotificationHandler<MessageAddedEvent>
{
    private readonly IEmbeddingGenerator _embeddingGenerator;
    private readonly IVectorMemoryRepository _vectorMemoryRepository;
    private readonly ILogger<PersistVectorMemoryHandler> _logger;

    public PersistVectorMemoryHandler(
        IEmbeddingGenerator embeddingGenerator,
        IVectorMemoryRepository vectorMemoryRepository,
        ILogger<PersistVectorMemoryHandler> logger)
    {
        _embeddingGenerator = embeddingGenerator;
        _vectorMemoryRepository = vectorMemoryRepository;
        _logger = logger;
    }

    public async Task Handle(MessageAddedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Generating embedding for MessageId {MessageId} in Session {SessionId}", notification.MessageId, notification.SessionId);
            
            var vector = await _embeddingGenerator.GenerateEmbeddingAsync(notification.Content, cancellationToken);
            
            await _vectorMemoryRepository.UpsertInteractionVectorAsync(
                notification.UserId,
                notification.SessionId,
                notification.Content,
                vector
            );

            _logger.LogInformation("Successfully persisted vector for MessageId {MessageId}", notification.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist vector memory for MessageId {MessageId}", notification.MessageId);
            // Exceptions here won't block the main MediatR response cycle if configured correctly, 
            // but even if it's awaited by default, at least we log and don't throw to disrupt the chat.
        }
    }
}
