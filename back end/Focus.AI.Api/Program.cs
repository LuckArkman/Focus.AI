using Focus.AI.Application;
using Focus.AI.Domain;
using Focus.AI.Infrastructure;
using MediatR;
using Focus.AI.Application.Commands.Ping;
using Focus.AI.Application.Commands.Auth.Register;
using Focus.AI.Application.Queries.Auth.Login;
using Focus.AI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.File(new CompactJsonFormatter(), "logs/focus-api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<Focus.AI.Api.ExceptionHandlers.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
        };
    });
builder.Services.AddAuthorization();

// DI Orchestration (Clean Architecture)
builder.Services.AddDomain();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (app.Environment.IsDevelopment())
    {
        await dbContext.Database.MigrateAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/test-crash", () => {
    throw new NullReferenceException("This is a deliberate crash for testing the global exception handler.");
})
.ExcludeFromDescription(); // Ocultar do swagger principal ou deixar .WithOpenApi()


// Auth Endpoints
app.MapPost("/api/auth/register", async (RegisterUserCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Ok(new { userId = result });
})
.WithName("Register")
.WithOpenApi();

app.MapPost("/api/auth/login", async (LoginQuery query, IMediator mediator) =>
{
    var token = await mediator.Send(query);
    return Results.Ok(new { token });
})
.WithName("Login")
.WithOpenApi();

// Exemplos Práticos
app.MapPost("/ping", async (PingCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Ok(new { message = result });
})
.WithName("PingEndpoint")
.RequireAuthorization()
.WithOpenApi();

app.Run();
