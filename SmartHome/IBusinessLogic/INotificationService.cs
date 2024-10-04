using Domain;
using IDataAccess;

namespace IBusinessLogic;

public interface INotificationService
{
    Notification GetNotificationById(int notificationId);
    
    List<Notification> GetNotificationsByFilter(Func<Notification, bool> filter, PageData pageData);
}