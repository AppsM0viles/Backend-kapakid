using FinTrackBack.Notifications.Application.DTOs;
using FinTrackBack.Notifications.Application.Features.CreateNotification;
using FinTrackBack.Notifications.Application.Features.GetNotifications;
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

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<List<NotificationDto>>> GetByUserId(Guid userId)
    {
        var result = await _mediator.Send(new GetNotificationsByUserIdQuery { UserId = userId });
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<NotificationDto> GetById(Guid id)
    {
        return NotFound();
    }
}