using Domain.Abstract;

namespace Model.Out;

public class GetDevicesResponse
{
    public List<GetDeviceResponse> Devices { get; set; }
    
    public GetDevicesResponse(List<Device> devices)
    {
        Devices = devices.Select(device => new GetDeviceResponse(device)).ToList();
    }

    public override bool Equals(object? obj)
    {
        return obj is GetDevicesResponse response &&
                Devices.SequenceEqual(response.Devices);
    }
}