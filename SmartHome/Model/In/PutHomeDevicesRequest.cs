using Domain;

namespace Model.In;

public class PutHomeDevicesRequest
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
                    IsConnected = deviceUnitRequest.IsConnected
                });
            }
        }

        return deviceUnits;
    }
}