using Domain;

namespace Model.In;

public class HomeDevicesRequest
{ 
    public List<DeviceUnitRequest>? DeviceUnits { get; set; }

    public List<DeviceUnit> ToEntity()
    {
        List<DeviceUnit> deviceUnits = new List<DeviceUnit>();

        foreach (DeviceUnitRequest deviceUnitRequest in DeviceUnits)
        {
            //  Device device = 
            deviceUnits.Add(new DeviceUnit()
            {
                //     DeviceId = deviceUnitRequest.DeviceId,
                IsConnected = deviceUnitRequest.IsConnected
            });
        }

        return deviceUnits;
    }
    

}