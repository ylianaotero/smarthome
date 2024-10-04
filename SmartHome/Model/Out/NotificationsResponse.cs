using Domain;
using Model.In;

namespace Model.Out;

public class NotificationsResponse
{
    public List<NotificationResponse> Notifications { get; set; }
    
    public NotificationsResponse(List<Notification> notifications)
    {
        Notifications = notifications.Select(notification => new NotificationResponse(notification)).ToList();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is NotificationsResponse response &&
               Notifications.SequenceEqual(response.Notifications);
    }
}