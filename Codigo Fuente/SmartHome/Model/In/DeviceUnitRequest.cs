namespace Model.In;

public class DeviceUnitRequest
{
    public long DeviceId { get; set; }
    
    public long? RoomId { get; set; }
    
    public string? RoomName { get; set; }
    public bool IsConnected { get; set; }
}