using FinTrackBack.Notifications.Domain.ValueObjects;

namespace FinTrackBack.Notifications.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }

    // Usuario dueño de la notificación (ajusta tipo según tu UserId)
    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;

    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }
}