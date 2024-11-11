using Domain.DTO;

namespace Model.In;

public class PatchDeviceUnitRequest
{
    public Guid HardwareId { get; set; }
    public bool? IsConnected { get; set; }
    public long? RoomId { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    
    public DeviceUnitDTO ToEntity()
    {
        return new DeviceUnitDTO()
        {
            HardwareId = HardwareId,
            IsConnected = IsConnected,
            RoomId = RoomId,
            Name = Name,
            Status = Status
        };
    }
}