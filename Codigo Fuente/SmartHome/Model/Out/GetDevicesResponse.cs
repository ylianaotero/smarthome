using Domain.Abstract;

namespace Model.Out;

public class GetDevicesResponse
{
    public List<GetDeviceResponse> Devices { get; set; }
    
    public int TotalCount { get; set; }
    
    public GetDevicesResponse(List<Device> devices, int count)
    {
        Devices = devices.Select(device => new GetDeviceResponse(device)).ToList();
        TotalCount = count; 
    }

    public override bool Equals(object? obj)
    {
        return obj is GetDevicesResponse response &&
                Devices.SequenceEqual(response.Devices);
    }
}