using Domain;

namespace BusinessLogic.IServices;

public interface IDeviceService
{
    List<Device> GetAllDevices();
    List<Device> GetDevicesByFilter(Func<Device, bool> filter);
}