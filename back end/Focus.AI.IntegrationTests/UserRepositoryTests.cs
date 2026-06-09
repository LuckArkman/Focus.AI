using FluentAssertions;
using Focus.AI.Domain.Entities;
using Focus.AI.Infrastructure.Data;
using Focus.AI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Focus.AI.IntegrationTests;

public class UserRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .Build();

    private ApplicationDbContext _dbContext = null!;
    private UserRepository _repository = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        
        // Aplica as migrations para criar o banco de dados
        await _dbContext.Database.MigrateAsync();

        _repository = new UserRepository(_dbContext);
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task AddAsync_WithValidUser_SavesToDatabase()
    {
        // Arrange
        var role = new Role { Id = Guid.NewGuid(), Name = "Admin" };
        _dbContext.Roles.Add(role);
        await _dbContext.SaveChangesAsync();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@focus.ai",
            PasswordHash = "hashedpassword",
            Name = "Test User",
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            RoleId = role.Id
        };

        // Act
        await _repository.AddAsync(user);

        // Assert
        var savedUser = await _repository.GetByEmailAsync("test@focus.ai");
        savedUser.Should().NotBeNull();
        savedUser!.Name.Should().Be("Test User");
        savedUser.Role.Name.Should().Be("Admin");
    }

    [Fact]
    public async Task AddAsync_WithDuplicateEmail_ThrowsException()
    {
        // Arrange
        var role = new Role { Id = Guid.NewGuid(), Name = "User" };
        _dbContext.Roles.Add(role);
        await _dbContext.SaveChangesAsync();

        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Email = "duplicate@focus.ai",
            PasswordHash = "hashedpassword",
            Name = "User 1",
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            RoleId = role.Id
        };
        await _repository.AddAsync(user1);

        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Email = "duplicate@focus.ai",
            PasswordHash = "hashedpassword",
            Name = "User 2",
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            RoleId = role.Id
        };

        // Act
        Func<Task> action = async () => await _repository.AddAsync(user2);

        // Assert
        await action.Should().ThrowAsync<DbUpdateException>();
    }
}
