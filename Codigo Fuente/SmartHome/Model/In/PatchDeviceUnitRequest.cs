using Domain.Concrete;

namespace Model.In;

public class PatchDeviceUnitRequest
{
    public string Name { get; set; }
    
    public DeviceUnitService ToEntity()
    {
        return new DeviceUnitService()
        {
            Name = Name
        };
    }

}