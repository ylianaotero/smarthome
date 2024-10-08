namespace Model.Out;

public class GetDeviceTypesResponse
{
    public List<string> DeviceTypes { get; set; }
    
    public override bool Equals(object? obj)
    {
        return obj is GetDeviceTypesResponse response &&
                DeviceTypes.SequenceEqual(response.DeviceTypes);
    }
}