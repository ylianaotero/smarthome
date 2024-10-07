namespace Domain.DTO;

public class NotificationDTO
{ 
    public string Event { get; set; }
    public long HomeId {get; set; }
    public Guid HardwareId {get; set; }
}