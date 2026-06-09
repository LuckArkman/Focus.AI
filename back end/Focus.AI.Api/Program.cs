using Focus.AI.Application;
using Focus.AI.Domain;
using Focus.AI.Infrastructure;
using MediatR;
using Focus.AI.Application.Commands.Ping;
using Focus.AI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Exemplos Práticos
app.MapPost("/ping", async (PingCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Ok(new { message = result });
})
.WithName("PingEndpoint")
.WithOpenApi();

app.Run();
