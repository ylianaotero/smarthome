using IDomain;
using Domain;

namespace DataAccess;

public class DevicesRepository
{
    public List<IDevice> GetDevicesByModel(long model)
    {
        SecurityCamera camera = new SecurityCamera()
        {
            Model = model
        };
        return new List<IDevice>()
        {
            camera
        };
        
    }
}