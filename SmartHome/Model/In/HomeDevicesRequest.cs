using Domain;

namespace Model.In;

public class HomeDevicesRequest
{ 
    public List<DeviceUnitRequest>? DeviceUnits { get; set; }

    public List<DeviceUnit> ToEntity()
    {
        List<DeviceUnit> deviceUnits = new List<DeviceUnit>();

        

        return deviceUnits;
    }
    

}