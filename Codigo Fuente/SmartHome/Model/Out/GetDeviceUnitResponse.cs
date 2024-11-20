using Domain.Concrete;

namespace Model.Out;

public class GetDeviceUnitResponse
{
    public string Name { get; set; }
    public Guid HardwareId { get; set; }
    public bool IsConnected { get; set; }
    public string Model { get; set; }
    public string Photo { get; set; }
    public string? RoomName { get; set; }
    public string? Status { get; set; }
    
    public GetDeviceUnitResponse(DeviceUnit deviceUnit)
    {
        Name = deviceUnit.Name;
        HardwareId = deviceUnit.HardwareId;
        IsConnected = deviceUnit.IsConnected;
        Model = deviceUnit.Device.Model;
        Photo = deviceUnit.Device.PhotoURLs[0];
        if (deviceUnit.Room != null)
        {
            RoomName = deviceUnit.Room.Name;
        }
        if (!String.IsNullOrEmpty(deviceUnit.Status))
        {
            Status = deviceUnit.Status;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is GetDeviceUnitResponse response &&
               Name == response.Name &&
               HardwareId.Equals(response.HardwareId) &&
               IsConnected == response.IsConnected &&
               Model == response.Model &&
               RoomName == response.RoomName &&
               Photo == response.Photo &&
               Status == response.Status;
    }
}