using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using Testcontainers.MongoDb;
using Testcontainers.Qdrant;
using Xunit;

namespace Focus.AI.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .Build();

    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder()
        .WithImage("mongo:6.0")
        .Build();

    private readonly QdrantContainer _qdrantContainer = new QdrantBuilder()
        .WithImage("qdrant/qdrant:v1.9.0")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var qdrantConnectionString = $"http://localhost:{_qdrantContainer.GetMappedPublicPort(6334)}";

            var testConfig = new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultConnection", _dbContainer.GetConnectionString() },
                { "MongoDbSettings:ConnectionString", _mongoContainer.GetConnectionString() },
                { "MongoDbSettings:DatabaseName", "FocusAI_Test" },
                { "ConnectionStrings:Qdrant", qdrantConnectionString }
            };

            configBuilder.AddInMemoryCollection(testConfig);
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _mongoContainer.StartAsync();
        await _qdrantContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await _mongoContainer.DisposeAsync();
        await _qdrantContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
