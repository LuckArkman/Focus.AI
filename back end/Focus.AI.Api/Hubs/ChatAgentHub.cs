using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Focus.AI.Api.Hubs;

[Authorize]
public class ChatAgentHub : Hub
{
    private readonly ILogger<ChatAgentHub> _logger;
    
    // A simple static dictionary to prevent multiple concurrent generations per user
    private static readonly ConcurrentDictionary<string, bool> _activeGenerations = new();

    public ChatAgentHub(ILogger<ChatAgentHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected to ChatAgentHub: {ConnectionId}, User: {UserId}", Context.ConnectionId, Context.UserIdentifier);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation(exception, "Client disconnected from ChatAgentHub: {ConnectionId}", Context.ConnectionId);
        
        // Cleanup potential hung generation lock
        if (Context.UserIdentifier != null)
        {
            _activeGenerations.TryRemove(Context.UserIdentifier, out _);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendPromptToAgent(string prompt, string sessionId)
    {
        var userId = Context.UserIdentifier ?? Context.User?.Identity?.Name ?? "anonymous";

        // Try to acquire the lock for this user
        if (!_activeGenerations.TryAdd(userId, true))
        {
            _logger.LogWarning("User {UserId} attempted concurrent generation.", userId);
            await Clients.Caller.SendAsync("ReceiveError", "You already have an active generation running.");
            return;
        }

        try
        {
            _logger.LogInformation("Received prompt from {UserId} for session {SessionId}: {Prompt}", userId, sessionId, prompt);
            
            // Simulating Token Streaming
            var tokens = new[] { "Thinking", "...", " Hello", "!", " I", " am", " your", " AI", " assistant." };
            
            foreach (var token in tokens)
            {
                await Task.Delay(200); // Simulate processing delay
                await Clients.Caller.SendAsync("ReceiveToken", token);
            }

            await Clients.Caller.SendAsync("GenerationCompleted", "Finished");
        }
        finally
        {
            // Release the lock
            _activeGenerations.TryRemove(userId, out _);
        }
    }
}
