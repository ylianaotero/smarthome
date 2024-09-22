namespace WebApi.Models.Out;

public class DevicesResponse
{
    public List<DeviceResponse> Devices { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is DevicesResponse response &&
                Devices.SequenceEqual(response.Devices);
    }
}