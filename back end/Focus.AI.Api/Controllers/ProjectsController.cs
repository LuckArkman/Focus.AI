using Focus.AI.Application.Commands.Projects.CreateProject;
using Focus.AI.Application.Queries.Projects.GetProjectsByUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Focus.AI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
    {
        var projectId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProjects), new { id = projectId }, new { projectId });
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var query = new GetProjectsByUserQuery();
        var projects = await _mediator.Send(query);
        return Ok(projects);
    }

    [HttpGet("{projectId}/status")]
    public async Task<IActionResult> GetProjectStatus(Guid projectId)
    {
        var query = new Focus.AI.Application.Queries.Projects.GetProjectStatus.GetProjectStatusQuery(projectId);
        var status = await _mediator.Send(query);
        return Ok(status);
    }

    [HttpGet("{projectId}/sessions")]
    public async Task<IActionResult> GetProjectSessions(Guid projectId)
    {
        var query = new Focus.AI.Application.Queries.ChatSessions.GetChatSessionsByProject.GetChatSessionsByProjectQuery(projectId);
        var sessions = await _mediator.Send(query);
        return Ok(sessions);
    }
}
