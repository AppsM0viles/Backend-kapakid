using FinTrackBack.Support.Application.DTOs;
using FinTrackBack.Support.Application.Features.CreateTicket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackBack.Support.Interfaces.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupportController : ControllerBase
{
    private readonly IMediator _mediator;

    public SupportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("tickets")]
    public async Task<ActionResult<SupportTicketDto>> Create([FromBody] CreateSupportTicketCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("tickets/{id:guid}")]
    public async Task<ActionResult<SupportTicketDto>> GetById(Guid id)
    {
        // TODO: implementar query de lectura
        return NotFound();
    }
}