using Domain.Concrete;

namespace Model.In;

public class GetDeviceUnitsRequest
{
    public long? RoomId { get; set; }
    public string? RoomName { get; set; }
    
    public Func<DeviceUnit, bool> ToFilter()
    {
        return deviceUnit => (RoomId == null || 
                              (deviceUnit.Room != null && deviceUnit.Room.Id == RoomId)) &&
                             (string.IsNullOrEmpty(RoomName) || 
                              (deviceUnit.Room != null && deviceUnit.Room.Name == RoomName));
    }
}