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
    
    public GetDeviceUnitResponse(DeviceUnit deviceUnitService)
    {
        Name = deviceUnitService.Device.Name;
        HardwareId = deviceUnitService.HardwareId;
        IsConnected = deviceUnitService.IsConnected;
        Model = deviceUnitService.Device.Model;
        Photo = deviceUnitService.Device.PhotoURLs[0];
        if (deviceUnitService.Room != null)
        {
            RoomName = deviceUnitService.Room.Name;
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
               Photo == response.Photo;
    }
}