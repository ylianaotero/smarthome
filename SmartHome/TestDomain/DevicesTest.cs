using Domain;
using Domain.Concrete;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestDomain;

[TestClass]
public class DevicesTest
{
    private SecurityCamera _securityCamera;
    private WindowSensor _windowSensor;
    
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
        _securityCamera = new SecurityCamera();
        _windowSensor = new WindowSensor();
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
        Company company = new Company(){};
        
        List<WindowSensorFunctionality> functionalities = new List<WindowSensorFunctionality> {WindowSensorFunctionality.OpenClosed};
        
        WindowSensor windowSensor1 = new WindowSensor()
        {
            Id = 1,
            Name = WindowSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = functionalities,
            Company = company
        };
        
        WindowSensor windowSensor2 = new WindowSensor()
        {
            Id = 2,
            Name = WindowSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = functionalities,
            Company = company
        };
        
        Assert.IsFalse(windowSensor1.Equals(windowSensor2));
    }
    
    [TestMethod]
    public void TestDifferentiationOfSecurityCamerasViaHardwardId()
    {
        Company company = new Company(){};
        
        List<SecurityCameraFunctionality> functionalities = new List<SecurityCameraFunctionality> {SecurityCameraFunctionality.MotionDetection};
        
        SecurityCamera securityCamera1 = new SecurityCamera()
        {
            Id = 1,
            Name = CameraName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = functionalities,
            Company = company
        };
        
        SecurityCamera securityCamera2 = new SecurityCamera()
        {
            Id = 2,
            Name = CameraName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = functionalities,
            Company = company
        };
        
        Assert.IsFalse(securityCamera1.Equals(securityCamera2));
    }
}