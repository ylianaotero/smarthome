using Domain.Concrete;

namespace Model.Out;

public class GetDeviceUnitsResponse
{
    public List<GetDeviceUnitResponse> DevicesUnit { get; set; }
    
    public GetDeviceUnitsResponse(List<DeviceUnit> devicesUnit)
    {
      DevicesUnit = devicesUnit.Select(deviceUnit => new GetDeviceUnitResponse(deviceUnit)).ToList();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetDeviceUnitsResponse response &&
                DevicesUnit.SequenceEqual(response.DevicesUnit);
    }
}