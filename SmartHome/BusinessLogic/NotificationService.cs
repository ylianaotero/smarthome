using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _notificationRepository;
    
    public NotificationService(IRepository<Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    
    
}