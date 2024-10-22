using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class DeviceUnitTest
{
    private DeviceUnit _deviceUnit;
    private SecurityCamera _securityCamera;
    private Guid _deviceHardwareId;
    
    private const string CameraName = "My Security Camera";
    private const string CameraCustomName = "Front Door Camera";
    private const string DeviceDescription = "This is a device";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const long DeviceModel = 1345354616346;
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
}