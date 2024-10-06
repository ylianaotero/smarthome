using Domain.Concrete;

namespace Model.Out;

public class GetDeviceUnitsResponse
{
    public List<DeviceUnit> DevicesUnit { get; set; }
    
    public GetDeviceUnitsResponse(List<DeviceUnit> devicesUnit)
    {
        DevicesUnit = devicesUnit;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetDeviceUnitsResponse response &&
                DevicesUnit.SequenceEqual(response.DevicesUnit);
    }
    
}