using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Focus.AI.Application.Commands.Auth.Register;
using Focus.AI.Application.Queries.Auth.Login;
using Focus.AI.Domain.Interfaces;
using Focus.AI.Application.Interfaces;
using Focus.AI.Domain.Entities;
using Focus.AI.Infrastructure.Data;

namespace Focus.AI.IntegrationTests;

public class GoldenPathTests : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IntegrationTestWebAppFactory _factory;
    private readonly HttpClient _client;

    public GoldenPathTests(IntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GoldenPath_ShouldRegisterLoginAndStoreDataInAllDatabases()
    {
        // 0. Seed Database (User Role is required for registration)
        using (var seedScope = _factory.Services.CreateScope())
        {
            var dbContext = seedScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (!dbContext.Roles.Any(r => r.Name == "User"))
            {
                dbContext.Roles.Add(new Role { Id = Guid.NewGuid(), Name = "User" });
                await dbContext.SaveChangesAsync();
            }
        }

        // 1. Register User (Hits Postgres)
        var email = $"golden{Guid.NewGuid()}@focus.ai";
        var password = "Password123!";

        var registerCommand = new RegisterUserCommand
        {
            Name = "Golden User",
            Email = email,
            Password = password
        };

        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerCommand);
        
        var errorString = !registerResponse.IsSuccessStatusCode ? await registerResponse.Content.ReadAsStringAsync() : "";
        registerResponse.IsSuccessStatusCode.Should().BeTrue($"User should be registered successfully, but got {errorString}");

        // 2. Login User
        var loginQuery = new LoginQuery { Email = email, Password = password };
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginQuery);
        loginResponse.IsSuccessStatusCode.Should().BeTrue("User should login successfully");

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        var token = loginResult.GetProperty("token").GetString();
        token.Should().NotBeNullOrEmpty("Token should be returned");

        // We know the API returns { userId = result } in /register
        var regResult = await registerResponse.Content.ReadFromJsonAsync<JsonElement>();
        var userIdString = regResult.GetProperty("userId").GetString();
        var userId = Guid.Parse(userIdString!);

        // 3. Insert into MongoDB and Qdrant via Scoped Services
        using var scope = _factory.Services.CreateScope();
        var chatRepo = scope.ServiceProvider.GetRequiredService<IChatSessionRepository>();
        var vectorRepo = scope.ServiceProvider.GetRequiredService<IVectorMemoryRepository>();

        // Insert Mongo
        var chatSession = new ChatSession
        {
            Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Messages = new List<ChatMessage>()
        };

        await chatRepo.CreateSessionAsync(chatSession);

        var message = new ChatMessage { SessionId = chatSession.Id, Role = "User", Content = "Hello AI" };
        await chatRepo.AddMessageToSessionAsync(chatSession.Id, message);

        // Insert Qdrant
        var testVector = new float[384];
        testVector[0] = 1.0f; testVector[1] = 0.5f; testVector[2] = 0.1f;
        await vectorRepo.UpsertInteractionVectorAsync(userId, chatSession.Id, "Hello AI", testVector);

        // Verify Mongo
        var history = (await chatRepo.GetSessionHistoryAsync(chatSession.Id, 10)).ToList();
        history.Should().NotBeEmpty("Message should be saved in MongoDB");
        history.First().Content.Should().Be("Hello AI");

        // Verify Qdrant
        var searchVector = new float[384];
        searchVector[0] = 1.0f; searchVector[1] = 0.4f; searchVector[2] = 0.1f;
        var similarContexts = await vectorRepo.SearchSimilarContextAsync(userId, searchVector, 1);
        similarContexts.Should().Contain("Hello AI", "Vector memory should be saved and retrieved from Qdrant");
    }
}
