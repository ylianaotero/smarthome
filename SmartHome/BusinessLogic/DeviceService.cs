using Domain;
using IDataAccess;

namespace BusinessLogic.Services;

public class DeviceService
{
    private readonly IRepository<Device> _deviceRepository;
    
    public DeviceService(IRepository<Device> deviceRepository)
    {
        _deviceRepository = deviceRepository;
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
        return new List<string> {"SecurityCamera", "WindowSensor"};
    }
    
}