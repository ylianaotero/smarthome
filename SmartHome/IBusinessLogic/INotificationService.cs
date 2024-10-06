using Domain.Concrete;
using Domain.DTO;
using IDataAccess;

namespace IBusinessLogic;

public interface INotificationService
{
    List<Notification> GetNotificationsByFilter(Func<Notification, bool> filter, PageData pageData);
    
    void SendNotifications(NotificationDTO notification);
}