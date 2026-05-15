using FinTrackBack.Notifications.Application.DTOs;
using FinTrackBack.Notifications.Domain.Entities;
using FinTrackBack.Notifications.Domain.Interfaces;
using FinTrackBack.Notifications.Domain.ValueObjects;
using MediatR;

namespace FinTrackBack.Notifications.Application.Features.CreateNotification;

public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand, NotificationDto>
{
    private readonly INotificationRepository _notificationRepository;

    public CreateNotificationHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Message = request.Message,
            Type = request.Type,
            Status = NotificationStatus.Unread,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _notificationRepository.AddAsync(notification, cancellationToken);

        return new NotificationDto
        {
            Id = created.Id,
            UserId = created.UserId,
            Title = created.Title,
            Message = created.Message,
            Type = created.Type,
            Status = created.Status,
            CreatedAt = created.CreatedAt
        };
    }
}