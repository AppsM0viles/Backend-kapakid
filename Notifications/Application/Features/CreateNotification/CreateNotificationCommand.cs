using FinTrackBack.Notifications.Application.DTOs;
using FinTrackBack.Notifications.Domain.ValueObjects;
using MediatR;

namespace FinTrackBack.Notifications.Application.Features.CreateNotification;

public record CreateNotificationCommand(
    Guid UserId,
    string Title,
    string Message,
    NotificationType Type
) : IRequest<NotificationDto>;