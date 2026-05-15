using FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;
using FinTrackBack.Notifications.Domain.Entities;
using FinTrackBack.Notifications.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinTrackBack.Notifications.Infrastructure.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly FinTrackBackDbContext _dbContext;

    public NotificationRepository(FinTrackBackDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Notification> AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return notification;
    }
}