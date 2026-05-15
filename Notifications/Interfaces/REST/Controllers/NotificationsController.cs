using FinTrackBack.Notifications.Application.DTOs;
using FinTrackBack.Notifications.Application.Features.CreateNotification;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackBack.Notifications.Interfaces.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<NotificationDto>> Create([FromBody] CreateNotificationCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // Stub simple para que CreatedAtAction no reviente
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NotificationDto>> GetById(Guid id)
    {
        // TODO: implementar query de lectura
        return NotFound();
    }
}