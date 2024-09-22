using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class DeviceService(IRepository<Device> deviceRepository) : IDeviceService
{
    public void CreateDevice(Device device)
    {
        deviceRepository.Add(device);
    }
    
    public List<Device> GetAllDevices()
    {
        return deviceRepository.GetAll();
    }
    
    public List<Device> GetDevicesByFilter(Func<Device, bool> filter)
    {
        return deviceRepository.GetByFilter(filter);
    }
    
    public List<string> GetDeviceTypes()
    {
        var deviceTypes = typeof(Device).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Device)) && !t.IsAbstract)
            .Select(t => t.Name)
            .ToList();

        return deviceTypes;
    }
    
}