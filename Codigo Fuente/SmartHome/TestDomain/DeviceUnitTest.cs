using CustomExceptions;
using Domain.Concrete;
using Domain.Enum;

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
    public void TestSetSmartLampStatus()
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
    public void TestExecuteWindowSensorAction()
    {
        string relatedFunctionality = "OpenClosed";
        string previousStatus = "Open";
        string newStatus = "Closed";
        
        WindowSensor windowSensor = new WindowSensor()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = new List<WindowSensorFunctionality>() { WindowSensorFunctionality.OpenClosed }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = windowSensor,
            Status = previousStatus
        };
        
        deviceUnit.ExecuteAction(relatedFunctionality);
        
        Assert.AreEqual(newStatus, deviceUnit.Status);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestExecuteWindowSensorActionFails()
    {
        string relatedFunctionality = "MotionDetection";
        string previousStatus = "Open";
        
        WindowSensor windowSensor = new WindowSensor()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = new List<WindowSensorFunctionality>() { WindowSensorFunctionality.OpenClosed }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = windowSensor,
            Status = previousStatus
        };
        
        deviceUnit.ExecuteAction(relatedFunctionality);
    }
    
    [TestMethod]
    public void TestExecuteSmartLampAction()
    {
        string relatedFunctionality = "OnOff";
        string previousStatus = "On";
        string newStatus = "Off";
        
        SmartLamp smartLamp = new SmartLamp()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = new List<SmartLampFunctionality>() { SmartLampFunctionality.OnOff }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = smartLamp,
            Status = previousStatus
        };
        
        deviceUnit.ExecuteAction(relatedFunctionality);
        
        Assert.AreEqual(newStatus, deviceUnit.Status);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestExecuteSmartLampActionFails()
    {
        string relatedFunctionality = "MotionDetection";
        string previousStatus = "On";
        
        SmartLamp smartLamp = new SmartLamp()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = new List<SmartLampFunctionality>() { SmartLampFunctionality.OnOff }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = smartLamp,
            Status = previousStatus
        };
        
        deviceUnit.ExecuteAction(relatedFunctionality);
    }
    
    [TestMethod]
    public void TestExecuteSecurityCameraMotionDetectionAction()
    {
        string relatedFunctionality = "MotionDetection";
        string previousStatus = "";
        string newStatus = "";
        
        SecurityCamera securityCamera = new SecurityCamera()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = new List<SecurityCameraFunctionality>() { SecurityCameraFunctionality.MotionDetection }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = securityCamera,
            Status = previousStatus
        };
        
        deviceUnit.ExecuteAction(relatedFunctionality);
        
        Assert.AreEqual(newStatus, deviceUnit.Status);
    }
    
    [TestMethod]
    public void TestExecuteSecurityCameraHumanDetectionAction()
    {
        string relatedFunctionality = "HumanDetection";
        string previousStatus = "";
        string newStatus = "";
        
        SecurityCamera securityCamera = new SecurityCamera()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = new List<SecurityCameraFunctionality>() { SecurityCameraFunctionality.HumanDetection }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = securityCamera,
            Status = previousStatus
        };
        
        deviceUnit.ExecuteAction(relatedFunctionality);
        
        Assert.AreEqual(newStatus, deviceUnit.Status);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestExecuteSecurityCameraActionFails()
    {
        string relatedFunctionality = "OnOff";
        string previousStatus = "";
        
        SecurityCamera securityCamera = new SecurityCamera()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = new List<SecurityCameraFunctionality>() { SecurityCameraFunctionality.MotionDetection }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = securityCamera,
            Status = previousStatus
        };
        
        deviceUnit.ExecuteAction(relatedFunctionality);
    }
    
    [TestMethod]
    public void TestExecuteMotionSensorAction()
    {
        string relatedFunctionality = "MotionDetection";
        string previousStatus = "";
        string newStatus = "";
        
        MotionSensor motionSensor = new MotionSensor()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = new List<MotionSensorFunctionality> { MotionSensorFunctionality.MotionDetection }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = motionSensor,
            Status = previousStatus
        };
        
        deviceUnit.ExecuteAction(relatedFunctionality);
        
        Assert.AreEqual(newStatus, deviceUnit.Status);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestExecuteMotionSensorActionFails()
    {
        string relatedFunctionality = "HumanDetection";
        string previousStatus = "";

        MotionSensor motionSensor = new MotionSensor()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = new List<MotionSensorFunctionality> { MotionSensorFunctionality.MotionDetection }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = motionSensor,
            Status = previousStatus
        };
        
        deviceUnit.ExecuteAction(relatedFunctionality);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestExecuteMotionSensorActionFailsBecauseFunctionalityIsNotSupported()
    {
        string relatedFunctionality = "MotionDetection";
        string previousStatus = "";

        MotionSensor motionSensor = new MotionSensor()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = motionSensor,
            Status = previousStatus
        };
        
        deviceUnit.ExecuteAction(relatedFunctionality);
    }

}