using Domain.Concrete;

namespace Model.Out;

public class GetDeviceUnitsResponse
{
    private List<GetDeviceUnitResponse> DevicesUnit { get; set; }
    
    public GetDeviceUnitsResponse(List<DeviceUnit>? devicesUnit)
    {
        if (devicesUnit == null)
        {
            DevicesUnit = new List<GetDeviceUnitResponse>();
            return;
        }
        
        DevicesUnit = devicesUnit.Select(deviceUnit => new GetDeviceUnitResponse(deviceUnit)).ToList();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetDeviceUnitsResponse response &&
                DevicesUnit.SequenceEqual(response.DevicesUnit);
    }
}