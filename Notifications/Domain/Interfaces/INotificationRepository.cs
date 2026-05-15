using FinTrackBack.Notifications.Domain.Entities;

namespace FinTrackBack.Notifications.Domain.Interfaces;

public interface INotificationRepository
{
    Task<Notification> AddAsync(Notification notification, CancellationToken cancellationToken = default);
}