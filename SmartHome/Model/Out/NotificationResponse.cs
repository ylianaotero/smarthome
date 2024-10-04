using Domain;

namespace Model.Out;

public class NotificationResponse
{
    public string Event { get;  set; }
    public DateTime CreatedAt { get;  set; }
    public bool Read { get;  set; }
    public DateTime ReadAt { get;  set; }
    
    public NotificationResponse(Notification notification)
    {
        Event = notification.Event;
        CreatedAt = notification.CreatedAt;
        Read = notification.Read;
        ReadAt = notification.ReadAt;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is NotificationResponse response &&
               Event == response.Event &&
               CreatedAt == response.CreatedAt &&
               Read == response.Read &&
               ReadAt == response.ReadAt;
    }
}