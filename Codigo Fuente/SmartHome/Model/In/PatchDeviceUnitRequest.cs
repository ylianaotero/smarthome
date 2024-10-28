using Domain.Concrete;

namespace Model.In;

public class PatchDeviceUnitRequest
{
    public string Name { get; set; }
    
    public DeviceUnit ToEntity()
    {
        return new DeviceUnit()
        {
            Name = Name
        };
    }

}