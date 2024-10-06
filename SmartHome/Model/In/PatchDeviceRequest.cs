using Domain.Concrete;

namespace Model.In;

public class PatchDeviceRequest
{
    public Guid DeviceUnitId { get; set; }
    public bool Status { get; set; }
    
    public DeviceUnit ToEntity()
    {
        return new DeviceUnit()
        {
            HardwareId = DeviceUnitId,
            IsConnected = Status
        };
    }
}