using Focus.AI.Application.Commands.ChatSessions.AddMessage;
using Focus.AI.Application.Commands.ChatSessions.CreateChatSession;
using Focus.AI.Application.Queries.ChatSessions.GetMessagesBySession;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Focus.AI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatSessionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatSessionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession([FromBody] CreateChatSessionCommand command)
    {
        var sessionId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetSessionMessages), new { sessionId }, new { sessionId });
    }

    [HttpPost("{sessionId}/messages")]
    public async Task<IActionResult> AddMessage(string sessionId, [FromBody] AddMessageCommand command)
    {
        // Enforce sessionId from route
        var routeCommand = command with { SessionId = sessionId };
        await _mediator.Send(routeCommand);
        return Ok();
    }

    [HttpGet("{sessionId}/messages")]
    public async Task<IActionResult> GetSessionMessages(string sessionId, [FromQuery] int take = 50)
    {
        var query = new GetMessagesBySessionQuery(sessionId, take);
        var messages = await _mediator.Send(query);
        return Ok(messages);
    }
}
