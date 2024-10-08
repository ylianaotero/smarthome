using Domain.Concrete;

namespace Model.Out;

public class GetNotificationsResponse
{
    public List<GetNotificationResponse> Notifications { get; set; }
    
    public GetNotificationsResponse(List<Notification> notifications)
    {
        Notifications = notifications.Select(notification => new GetNotificationResponse(notification)).ToList();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetNotificationsResponse response &&
               Notifications.SequenceEqual(response.Notifications);
    }
}