using IDomain;

namespace DataAccess;

public class DevicesRepository
{
    private SmartHomeContext _database;
    
    public DevicesRepository(SmartHomeContext database)
    {
        _database = database;
    }
    
    public List<Device> GetDevicesByModel(long model)
    {
        return _database.Devices
            .Where(device => device.Model == model).ToList();
    }
}