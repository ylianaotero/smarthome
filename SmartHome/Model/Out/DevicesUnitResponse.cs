using Domain.Concrete;

namespace Model.Out;

public class DevicesUnitResponse
{
    public List<DeviceUnit> DevicesUnit { get; set; }
    
    public DevicesUnitResponse(List<DeviceUnit> devicesUnit)
    {
        DevicesUnit = devicesUnit;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is DevicesUnitResponse response &&
                DevicesUnit.SequenceEqual(response.DevicesUnit);
    }
    
}