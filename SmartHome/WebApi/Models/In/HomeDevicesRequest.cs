using Domain;

namespace WebApi.Models.In;

public class HomeDevicesRequest
{ 
    public List<WindowSensorRequest>? WindowSensors { get; set; }
    public List<SecurityCameraRequest>? SecurityCameras { get; set; }

    public List<Device> ToEntity()
    {
        List<Device> devices = new List<Device>();
        if(WindowSensors != null && SecurityCameras != null)
        {
            foreach (var windowSensor in WindowSensors)
            {
                devices.Add(windowSensor.ToEntity());
            }
            foreach (var securityCamera in SecurityCameras)
            {
                devices.Add(securityCamera.ToEntity());
            }
        }
        return devices;
    }
    
}