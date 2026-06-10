using Focus.AI.Application.Interfaces.Authentication;
using Focus.AI.Domain.Interfaces;
using Focus.AI.Infrastructure.Authentication;
using Focus.AI.Infrastructure.Data;
using Focus.AI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qdrant.Client;

namespace Focus.AI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));
        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IChatSessionRepository, ChatSessionRepository>();

        // Qdrant
        var qdrantUrl = configuration.GetConnectionString("Qdrant");
        var uri = new Uri(qdrantUrl!);
        services.AddSingleton(new QdrantClient(uri.Host, uri.Port));
        services.AddHostedService<QdrantInitializer>();
        services.AddScoped<Focus.AI.Application.Interfaces.IVectorMemoryRepository, VectorMemoryRepository>();

        return services;
    }
}
