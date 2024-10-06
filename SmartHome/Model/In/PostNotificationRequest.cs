using Domain.DTO;

namespace Model.In;

public class PostNotificationRequest
{
    public string Event { get; set; }
    public long HomeId { get; set; }
    public long HardwareId { get; set; }
    
    public NotificationDTO ToEntity()
    {
        return new NotificationDTO
        {
            Event = this.Event,
            HomeId = this.HomeId,
            HardwareId = this.HardwareId
        };
    }
}