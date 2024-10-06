using Domain.Concrete;

namespace Model.Out;

public class GetNotificationResponse
{
    public string Event { get;  set; }
    public DateTime CreatedAt { get;  set; }
    public bool Read { get;  set; }
    public DateTime ReadAt { get;  set; }
    
    public GetNotificationResponse(Notification notification)
    {
        Event = notification.Event;
        CreatedAt = notification.CreatedAt;
        Read = notification.Read;
        ReadAt = notification.ReadAt;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetNotificationResponse response &&
               Event == response.Event &&
               CreatedAt == response.CreatedAt &&
               Read == response.Read &&
               ReadAt == response.ReadAt;
    }
}