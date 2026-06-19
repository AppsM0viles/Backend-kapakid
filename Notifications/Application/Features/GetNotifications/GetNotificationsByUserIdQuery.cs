using MediatR;
using FinTrackBack.Notifications.Application.DTOs;
using FinTrackBack.Notifications.Domain.Interfaces;

namespace FinTrackBack.Notifications.Application.Features.GetNotifications;

public class GetNotificationsByUserIdQuery : IRequest<List<NotificationDto>>
{
    public Guid UserId { get; set; }
}

public class GetNotificationsByUserIdQueryHandler : IRequestHandler<GetNotificationsByUserIdQuery, List<NotificationDto>>
{
    private readonly INotificationRepository _repository;

    public GetNotificationsByUserIdQueryHandler(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<NotificationDto>> Handle(GetNotificationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _repository.GetByUserIdAsync(request.UserId);
        
        return notifications.Select(n => new NotificationDto
        {
            Id = n.Id,
            UserId = n.UserId,
            Title = n.Title,
            Message = n.Message,
            Type = n.Type,
            Status = n.Status,
            CreatedAt = n.CreatedAt
        }).ToList();
    }
}