using Domain.Concrete;

namespace Model.In;

public class PatchDeviceRequest
{
    public Guid HardwareId { get; set; }
    public bool IsConnected { get; set; }
    
    public DeviceUnitService ToEntity()
    {
        return new DeviceUnitService()
        {
            HardwareId = HardwareId,
            IsConnected = IsConnected
        };
    }
}