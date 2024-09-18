using BusinessLogic.IServices;
using Domain;
using IDataAccess;

namespace BusinessLogic.Services;

public class DeviceService : IDeviceService
{
    private readonly IRepository<Device> _deviceRepository;
    
    public DeviceService(IRepository<Device> deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }
    
    public void CreateWindowSensor(WindowSensor windowSensor)
    {
        _deviceRepository.Add(windowSensor);
    }
    
    public List<Device> GetAllDevices()
    {
        return _deviceRepository.GetAll();
    }
    
    public List<Device> GetDevicesByFilter(Func<Device, bool> filter)
    {
        return _deviceRepository.GetByFilter(filter);
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