using Domain;

namespace Model.Out;

public class NotificationResponse
{
    public string Event { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool Read { get; private set; }
    public DateTime ReadAt { get; private set; }
    
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