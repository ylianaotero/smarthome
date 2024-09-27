using Domain;

namespace WebApi.Models.In;

public class HomeDevicesRequest
{ 
    public List<DeviceRequest> Devices { get; set; }
    
    public List<WindowSensorRequest> WindowSensors { get; set; }
    public List<SecurityCameraRequest> SecurityCameras { get; set; }

    public List<Device> ToEntity()
    {
        List<Device> devices = new List<Device>();
        foreach (var windowSensor in WindowSensors)
        {
            devices.Add(windowSensor.ToEntity());
        }
        foreach (var securityCamera in SecurityCameras)
        {
            devices.Add(securityCamera.ToEntity());
        }
        return devices;
    }
    
}