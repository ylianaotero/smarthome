using CustomExceptions;
using Domain.Abstract;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class DeviceService(IRepository<Device> deviceRepository) : IDeviceService
{
    private const string DeviceNotFoundMessage = "Device not found";
    
    public void CreateDevice(Device device)
    {
        deviceRepository.Add(device);
    }
    
    public Device GetDeviceById(long id)
    {
        Device device = deviceRepository.GetById(id);
        
        if (device == null)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
        
        return device;
    }
    
    public List<Device> GetDevicesByFilter(Func<Device, bool> filter, PageData pageData)
    {
        return deviceRepository.GetByFilter(filter, pageData);
    }
    
    public List<string> GetDeviceTypes()
    {
        List<string> deviceTypes = typeof(Device).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Device)) && !t.IsAbstract)
            .Select(t => t.Name)
            .ToList();

        return deviceTypes;
    }
}