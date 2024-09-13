using IDomain;

namespace DataAccess;

public class DevicesRepository
{
    private SmartHomeContext _database;
    
    public DevicesRepository(SmartHomeContext database)
    {
        _database = database;
    }
    
    public List<Device> GetDevicesByFilter(Func<Device, bool> predicate)
    {
        return _database.Devices
            .Where(predicate).ToList();
    }
}