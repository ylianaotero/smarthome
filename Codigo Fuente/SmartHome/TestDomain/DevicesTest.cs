using Domain.Concrete;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestDomain;

[TestClass]
public class DevicesTest
{
    private SecurityCamera _securityCamera;
    private SecurityCamera _securityCamera1;
    private SecurityCamera _securityCamera2;
    private WindowSensor _windowSensor;
    private WindowSensor _windowSensor1;
    private WindowSensor _windowSensor2;
    private Company _company;
    private List<SecurityCameraFunctionality> _cameraFunctionalities;
    private List<WindowSensorFunctionality> _windowSensorfunctionalities;
    
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string DeviceDescription = "This is a device";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const long DeviceModel = 1345354616346;
    
    private const string WindowSensorType = "WindowSensor";
    private const string SecurityCameraType = "SecurityCamera";
    
    [TestInitialize]
    public void TestInitialize()
    {
        _company = new Company();
        _windowSensorfunctionalities = new List<WindowSensorFunctionality> {WindowSensorFunctionality.OpenClosed};
        _cameraFunctionalities = new List<SecurityCameraFunctionality> {SecurityCameraFunctionality.MotionDetection};

        InitializeDevices();
    }

    private void InitializeDevices()
    {
        _securityCamera = new SecurityCamera();
        _windowSensor = new WindowSensor();
        _windowSensor1 = new WindowSensor()
        {
            Id = 1,
            Name = WindowSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _windowSensorfunctionalities,
            Company = _company
        };
        _windowSensor2 = new WindowSensor()
        {
            Id = 2,
            Name = WindowSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _windowSensorfunctionalities,
            Company = _company
        };
        
        _securityCamera1 = new SecurityCamera()
        {
            Id = 1,
            Name = CameraName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _cameraFunctionalities,
            Company = _company
        };
        _securityCamera2 = new SecurityCamera()
        {
            Id = 2,
            Name = CameraName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _cameraFunctionalities,
            Company = _company
        };
    }
    
    [TestMethod]
    public void TestAddNameToSecurityCamera()
    {
        _securityCamera.Name = CameraName;
        
        Assert.AreEqual(CameraName, _securityCamera.Name);
    }
    
    [TestMethod]
    public void TestAddModelToSecurityCamera()
    {
        _securityCamera.Model = DeviceModel;
        
        Assert.AreEqual(DeviceModel, _securityCamera.Model);
    }
    
    [TestMethod]
    public void TestAddPhotosToSecurityCamera()
    {
        List<string> photos = new List<string> { DevicePhotoUrl, DevicePhotoUrl };
        
        _securityCamera.PhotoURLs = photos;
        
        Assert.AreEqual(photos, _securityCamera.PhotoURLs);
    }

    [TestMethod]
    public void TestAddCompanyDataToSecurityCamera()
    {
        Company company = new Company(){};
        
        _securityCamera.Company = company;
        
        Assert.AreEqual(company, _securityCamera.Company);
    }
    
    [TestMethod]
    public void TestAddDescriptionToSecurityCamera()
    {
        _securityCamera.Description = DeviceDescription;
        
        Assert.AreEqual(DeviceDescription, _securityCamera.Description);
    }
    
    [TestMethod]
    public void TestAddIndoorLocationTypeToSecurityCamera()
    {
        LocationType locationType = LocationType.Indoor;
        
        _securityCamera.LocationType = locationType;
        
        Assert.AreEqual(locationType, _securityCamera.LocationType);
    }

    [TestMethod]
    public void TestAddOutdoorLocationTypeToSecurityCamera()
    {
        LocationType locationType = LocationType.Outdoor;
        
        _securityCamera.LocationType = locationType;
        
        Assert.AreEqual(locationType, _securityCamera.LocationType);
    }
    
    [TestMethod]
    public void TestAddMovementDetectionFunctionalityToSecurityCamera()
    {
        List<SecurityCameraFunctionality>? functionalities = new List<SecurityCameraFunctionality> 
            {SecurityCameraFunctionality.MotionDetection};
        
        _securityCamera.Functionalities = functionalities;
        
        Assert.AreEqual(functionalities, _securityCamera.Functionalities);
    }
    
    [TestMethod]
    public void TestAddHumanDetectionFunctionalityToSecurityCamera()
    {
        List<SecurityCameraFunctionality>? functionalities = new List<SecurityCameraFunctionality> 
            {SecurityCameraFunctionality.HumanDetection};
        
        _securityCamera.Functionalities = functionalities;
        
        Assert.AreEqual(functionalities, _securityCamera.Functionalities);
    }
    
    [TestMethod]
    public void TestAddOpenClosedFunctionalityToWindowSensor()
    {
        List<WindowSensorFunctionality>? functionalities = new List<WindowSensorFunctionality> 
            {WindowSensorFunctionality.OpenClosed};
        
        _windowSensor.Functionalities = functionalities;
        
        Assert.AreEqual(_windowSensor.Functionalities, functionalities);
    }
    
    [TestMethod]
    public void TestAddDeviceCharacteristicsToWindowSensor()
    {
        _windowSensor = new WindowSensor() 
        {
            Name = WindowSensorName,
        };
        
        Assert.IsNotNull(_windowSensor);
    }
    
    [TestMethod]
    public void TestWindowSensorTypeIsCorrect()
    {
        _windowSensor = new WindowSensor(){};
        
        Assert.AreEqual(WindowSensorType, _windowSensor.Kind);
    }
    
    [TestMethod]
    public void TestSecurityCameraTypeIsCorrect()
    {
        _securityCamera = new SecurityCamera(){};
        
        Assert.AreEqual(SecurityCameraType, _securityCamera.Kind);
    }

    [TestMethod]
    public void TestDifferentiationOfWindowSensorsViaHardwardId()
    {
        Assert.IsFalse(_windowSensor1.Equals(_windowSensor2));
    }
    
    [TestMethod]
    public void TestDifferentiationOfSecurityCamerasViaHardwardId()
    {
        Assert.IsFalse(_securityCamera1.Equals(_securityCamera2));
    }

    [TestMethod]
    public void TestCreateMotionSensor()
    {
        MotionSensor motionSensor = new MotionSensor() {Id = 1};
        
        Assert.AreEqual(1, motionSensor.Id);
    }
}