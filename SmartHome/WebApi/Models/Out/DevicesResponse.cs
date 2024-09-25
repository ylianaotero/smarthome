using Domain;

namespace WebApi.Models.Out;

public class DevicesResponse
{
    public List<DeviceResponse> Devices { get; set; }
    
    public DevicesResponse(List<Device> devices)
    {
        Devices = devices.Select(device => new DeviceResponse(device)).ToList();
    }

    public override bool Equals(object? obj)
    {
        return obj is DevicesResponse response &&
                Devices.SequenceEqual(response.Devices);
    }
}