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
    
}