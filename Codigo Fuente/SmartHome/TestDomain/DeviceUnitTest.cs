using CustomExceptions;
using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class DeviceUnitTest
{
    private DeviceUnit _deviceUnit;
    private SecurityCamera _securityCamera;
    private Room _room;
    private Guid _deviceHardwareId;
    
    private const string CameraName = "My Security Camera";
    private const string SmartLampName = "My Smart Lamp";
    private const string CameraCustomName = "Front Door Camera";
    private const string DeviceDescription = "This is a device";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const string DeviceModel = "1345354616346";
    private const string RoomName = "Bedroom";
    private const long RoomId = 1;
    private const bool DeviceIsConnected = true;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _securityCamera = new SecurityCamera()
        {
            Name = CameraName,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Model = DeviceModel
        };
        
        _deviceHardwareId = Guid.NewGuid();
        
        _deviceUnit = new DeviceUnit() {};
        
        _room = new Room()
        {
            Id = RoomId,
            Name = RoomName
        };
    }
    
    [TestMethod]
    public void TestConnectDeviceUnit()
    {
        _deviceUnit.IsConnected = DeviceIsConnected;
        
        Assert.IsTrue(_deviceUnit.IsConnected);
    }
    
    [TestMethod]
    public void TestAddDeviceToUnit()
    {
        _deviceUnit.Device = _securityCamera;
        
        Assert.AreEqual(_securityCamera, _deviceUnit.Device);
    }
    
    [TestMethod]
    public void TestAddHardwareIdToDeviceUnit()
    {
        _deviceUnit.HardwareId = _deviceHardwareId;
        
        Assert.AreEqual(_deviceHardwareId, _deviceUnit.HardwareId);
    }
    
    [TestMethod]
    public void TestAddNameToDeviceUnit()
    {
        _deviceUnit.Name = CameraCustomName;
        
        Assert.AreEqual(CameraCustomName, _deviceUnit.Name);
    }
    
    [TestMethod]
    public void TestAddRoomToDeviceUnit()
    {
        _deviceUnit.Room = _room;
        
        Assert.AreEqual(_deviceUnit.Room, _room);
    }
    
    
    [TestMethod]
    public void TestSetDeviceUnitStatus()
    {
        string status = "On";
        
        SmartLamp smartLamp1 = new SmartLamp()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = smartLamp1
        };
        
        deviceUnit.Status = status;
        
        Assert.AreEqual(status, deviceUnit.Status);
    }

    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestSetSmartLampStatusFails()
    {
        string status = "Open";
        
        SmartLamp smartLamp1 = new SmartLamp()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = smartLamp1
        };
        
        deviceUnit.Status = status;
    }
    
}