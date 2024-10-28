using Domain.Abstract;
using Domain.Concrete;

namespace TestDomain;

public class RoomTests
{
    [TestMethod]
    public void TestAddDeviceUnitToRoom()
    {
        Room room = new Room()
        {
            Id = 1,
            Name = "Bedroom"
        };

        Device device = new WindowSensor();
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            HardwareId = Guid.NewGuid(),
            Device = device,
            Room = room
        };
    }
}