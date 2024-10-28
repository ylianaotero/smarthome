using Domain.Abstract;
using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class RoomTests
{
    private const string RoomName = "Bedroom";
    private const long RoomId = 1;
    
    private Room _room;
    private Device _device;
    private DeviceUnit _deviceUnit;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _room = new Room()
        {
            Id = RoomId,
            Name = RoomName
        };
        
        _device = new WindowSensor();
        
        _deviceUnit = new DeviceUnit()
        {
            HardwareId = Guid.NewGuid(),
            Device = _device,
        };
    }
    
    [TestMethod]
    public void TestAddDeviceUnitToRoom()
    {
        _deviceUnit.Room = _room;
        
        Assert.AreEqual(_deviceUnit.Room, _room);
    }
}