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
    
}