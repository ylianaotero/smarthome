using Domain.Concrete;

namespace Model.In;

public class PatchDeviceRequest
{
    public Guid HardwareId { get; set; }
    public bool IsConnected { get; set; }
    public long? RoomId { get; set; }
    
    public DeviceUnit ToEntity()
    {
        return new DeviceUnit()
        {
            HardwareId = HardwareId,
            IsConnected = IsConnected
        };
    }
}