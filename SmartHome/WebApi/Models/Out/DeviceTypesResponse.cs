namespace WebApi.Models.Out;

public class DeviceTypesResponse
{
    public List<string> DeviceTypes { get; set; }
    
    public override bool Equals(object? obj)
    {
        return obj is DeviceTypesResponse response &&
                DeviceTypes.SequenceEqual(response.DeviceTypes);
    }
}