using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using System.Text.Json;

Console.WriteLine("=== Focus.AI SignalR Client Test ===");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:5075"); // API Http Port

var email = $"test{Guid.NewGuid()}@focus.ai";
var password = "Password123!";

Console.WriteLine("1. Registering user...");
var regResponse = await httpClient.PostAsJsonAsync("/api/auth/register", new { 
    Name = "SignalR Tester",
    Email = email,
    Password = password
});

if (!regResponse.IsSuccessStatusCode)
{
    Console.WriteLine("Failed to register. Is API running?");
    var error = await regResponse.Content.ReadAsStringAsync();
    Console.WriteLine(error);
    return;
}

Console.WriteLine("2. Logging in...");
var loginResponse = await httpClient.PostAsJsonAsync("/api/auth/login", new {
    Email = email,
    Password = password
});

if (!loginResponse.IsSuccessStatusCode)
{
    Console.WriteLine("Failed to login.");
    return;
}

var loginData = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
var token = loginData.GetProperty("token").GetString()!;

Console.WriteLine($"Token acquired: {token[..20]}...");

Console.WriteLine("3. Connecting to SignalR Hub...");
var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5075/agent-hub", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult(token)!;
    })
    .WithAutomaticReconnect()
    .Build();

connection.On<string>("ReceiveToken", tokenChunk =>
{
    Console.Write(tokenChunk);
});

connection.On<string>("ReceiveError", error =>
{
    Console.WriteLine($"\n[ERROR]: {error}");
});

connection.On<string>("GenerationCompleted", msg =>
{
    Console.WriteLine($"\n[COMPLETED]: {msg}");
});

await connection.StartAsync();
Console.WriteLine("Connected! Status: " + connection.State);

Console.WriteLine("4. Sending Prompt...");
await connection.InvokeAsync("SendPromptToAgent", "Write a hello world", Guid.NewGuid().ToString());

// Wait a bit to receive all tokens
await Task.Delay(3000);

Console.WriteLine("\nDisconnecting...");
await connection.StopAsync();
Console.WriteLine("Done.");
