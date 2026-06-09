namespace Focus.AI.Domain.Entities;

public class ChatMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SessionId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // User, Assistant, System
    public string Content { get; set; } = string.Empty;
    public int TokensUsed { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
