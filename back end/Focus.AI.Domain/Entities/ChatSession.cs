namespace Focus.AI.Domain.Entities;

public class ChatSession
{
    public string Id { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid? ProjectId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<ChatMessage> Messages { get; set; } = new();
}
