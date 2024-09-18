using Domain;

namespace BusinessLogic.IServices;

public interface IDeviceService
{
    void CreateWindowSensor(WindowSensor windowSensor);
    List<Device> GetAllDevices();
    List<Device> GetDevicesByFilter(Func<Device, bool> filter);
    List<string> GetDeviceTypes();
}