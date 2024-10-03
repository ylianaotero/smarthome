using CustomExceptions;
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
    
    public void CreateNotification(Notification notification)
    {
        _notificationRepository.Add(notification);
    }

    public List<Notification> GetNotifications(PageData pageData)
    {
        return _notificationRepository.GetAll(pageData);
    }
    
    public Notification GetNotificationById(int id)
    {
        Notification notification = _notificationRepository.GetById(id);
        if(notification == null)
        {
            throw new ElementNotFound("Notification not found");
        }
        return notification;
    }
}