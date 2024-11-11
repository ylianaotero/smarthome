using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class DeviceUnitTest
{
    private DeviceUnit _deviceUnitService;
    private SecurityCamera _securityCamera;
    private Room _room;
    private Guid _deviceHardwareId;
    
    private const string CameraName = "My Security Camera";
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
        
        _deviceUnitService = new DeviceUnit() {};
        
        _room = new Room()
        {
            Id = RoomId,
            Name = RoomName
        };
    }
    
    [TestMethod]
    public void TestConnectDeviceUnit()
    {
        _deviceUnitService.IsConnected = DeviceIsConnected;
        
        Assert.IsTrue(_deviceUnitService.IsConnected);
    }
    
    [TestMethod]
    public void TestAddDeviceToUnit()
    {
        _deviceUnitService.Device = _securityCamera;
        
        Assert.AreEqual(_securityCamera, _deviceUnitService.Device);
    }
    
    [TestMethod]
    public void TestAddHardwareIdToDeviceUnit()
    {
        _deviceUnitService.HardwareId = _deviceHardwareId;
        
        Assert.AreEqual(_deviceHardwareId, _deviceUnitService.HardwareId);
    }
    
    [TestMethod]
    public void TestAddNameToDeviceUnit()
    {
        _deviceUnitService.Name = CameraCustomName;
        
        Assert.AreEqual(CameraCustomName, _deviceUnitService.Name);
    }
    
    [TestMethod]
    public void TestAddRoomToDeviceUnit()
    {
        _deviceUnitService.Room = _room;
        
        Assert.AreEqual(_deviceUnitService.Room, _room);
    }
    
    
    [TestMethod]
    public void TestSetDeviceUnitStatus()
    {
        _deviceUnitService.Status = "On";
        
        Assert.AreEqual("On", _deviceUnitService.Status);
    }
}