using Domain.Concrete;

namespace Model.Out;

public class GetNotificationResponse
{
    public Guid HardwareId { get; set; }
    public string DeviceKind { get; set; }
    public string Event { get;  set; }
    public DateTime CreatedAt { get;  set; }
    public bool Read { get;  set; }
    public DateTime? ReadAt { get;  set; }
    
    public GetNotificationResponse(Notification notification)
    {
        HardwareId = notification.DeviceUnit.HardwareId;
        DeviceKind = notification.DeviceUnit.Device.Kind;
        Event = notification.Event;
        CreatedAt = notification.CreatedAt;
        Read = notification.Read;
        ReadAt = notification.ReadAt.Value;
    }
}