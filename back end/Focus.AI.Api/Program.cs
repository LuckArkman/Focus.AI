using Focus.AI.Application;
using Focus.AI.Domain;
using Focus.AI.Infrastructure;
using MediatR;
using Focus.AI.Application.Commands.Ping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI Orchestration (Clean Architecture)
builder.Services.AddDomain();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

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
