namespace Domain;

public class NotificationDTO
{
    public long Id { get; set; }
    public string Event { get; set; }
    public long HomeId {get; set; }
    public long HardwareId {get; set; }

}