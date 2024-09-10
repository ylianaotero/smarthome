using Domain;

namespace IService;

public interface INotificationService
{
    void AddNotification(Notification notification);
    IEnumerable<Notification> GetNotifications();
    IEnumerable<Notification> FilterByDevice(string device);
    IEnumerable<Notification> FilterByDate(DateTime date);
    IEnumerable<Notification> FilterByReadStatus(bool isRead);
}