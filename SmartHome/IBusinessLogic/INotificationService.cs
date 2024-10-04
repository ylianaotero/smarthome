using Domain;

namespace IBusinessLogic;

public interface INotificationService
{
    Notification GetNotificationById(int notificationId);
}