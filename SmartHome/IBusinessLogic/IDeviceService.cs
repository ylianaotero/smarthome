using Domain;

namespace IBusinessLogic;

public interface IDeviceService
{
    void CreateDevice(Device device);
    List<Device> GetAllDevices();
    List<Device> GetDevicesByFilter(Func<Device, bool> filter);
    List<string> GetDeviceTypes();
}