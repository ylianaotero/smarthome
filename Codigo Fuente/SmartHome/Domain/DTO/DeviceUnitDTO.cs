namespace Domain.DTO;

public class DeviceUnitDTO
{
    public long? DeviceId { get; set; }
    public bool? IsConnected { get; set; }
    public Guid HardwareId { get; set; }
    public long? RoomId { get; set; }
    public string? Name { get; set; }
}