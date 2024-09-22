using BusinessLogic.IServices;
using DataAccess.Exceptions;
using Domain;
using IDataAccess;

namespace BusinessLogic.Services;

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
            throw new ElementNotFoundException(DeviceNotFoundMessage);
        }
        
        return device;
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