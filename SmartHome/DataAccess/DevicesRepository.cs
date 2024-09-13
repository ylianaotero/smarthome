using DataAccess.Exceptions;
using Domain;
using IDomain;

namespace DataAccess;

public class DevicesRepository
{
    private SmartHomeContext _database;
    
    public DevicesRepository(SmartHomeContext database)
    {
        _database = database;
    }
    
    public void AddDevice(Device device)
    {
        _database.Devices.Add(device);
        _database.SaveChanges();
    }
    
    public Device? GetDeviceById(long id)
    {
        return _database.Devices.Find(id);
    }

    public List<Device> GetAllDevices()
    {
        return _database.Devices.ToList();
    }
    
    public List<Device> GetDevicesByFilter(Func<Device, bool> predicate)
    {
        return _database.Devices
            .Where(predicate).ToList();
    }
    
    public void DeleteDevice(Device device)
    {
        throw new ElementNotFoundException("Device not found");
        
        _database.Devices.Remove(device);
        _database.SaveChanges();
    }
}