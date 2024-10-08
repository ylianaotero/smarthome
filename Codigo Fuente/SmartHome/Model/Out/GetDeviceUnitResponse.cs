using Domain.Concrete;

namespace Model.Out;

public class GetDeviceUnitResponse
{
    public string Name { get; set; }
    public Guid HardwareId { get; set; }
    public bool IsConnected { get; set; }
    public long Model { get; set; }
    public string Photo { get; set; }
    
    public GetDeviceUnitResponse(DeviceUnit deviceUnit)
    {
        Name = deviceUnit.Device.Name;
        HardwareId = deviceUnit.HardwareId;
        IsConnected = deviceUnit.IsConnected;
        Model = deviceUnit.Device.Model;
        Photo = deviceUnit.Device.PhotoURLs[0];
    }

    public override bool Equals(object? obj)
    {
        return obj is GetDeviceUnitResponse response &&
               Name == response.Name &&
               HardwareId.Equals(response.HardwareId) &&
               IsConnected == response.IsConnected &&
               Model == response.Model &&
               Photo == response.Photo;
    }
}