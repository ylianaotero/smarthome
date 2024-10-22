using Domain.Concrete;

namespace Model.In;

public class PatchDeviceUnitRequest
{
    public Guid HardwareId { get; set; }
    public string Name { get; set; }
    
    public DeviceUnit ToEntity()
    {
        return new DeviceUnit()
        {
            HardwareId = HardwareId,
            Name = Name
        };
    }

}