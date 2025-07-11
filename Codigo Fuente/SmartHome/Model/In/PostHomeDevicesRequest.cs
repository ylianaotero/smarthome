using Domain.DTO;

namespace Model.In;

public class PostHomeDevicesRequest
{ 
    public List<DeviceUnitRequest>? DeviceUnits { get; set; }

    public List<DeviceUnitDTO> ToEntity()
    {
        List<DeviceUnitDTO> deviceUnits = new List<DeviceUnitDTO>();
        
        if (DeviceUnits != null)
        {
            foreach (DeviceUnitRequest deviceUnitRequest in DeviceUnits)
            {
                deviceUnits.Add(new DeviceUnitDTO
                {
                    DeviceId = deviceUnitRequest.DeviceId,
                    RoomId = deviceUnitRequest.RoomId, 
                    RoomName = deviceUnitRequest.RoomName, 
                    IsConnected = deviceUnitRequest.IsConnected
                });
            }
        }

        return deviceUnits;
    }
}