using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;


namespace TestDomain;

[TestClass]
public class DevicesTest
{
    private SecurityCamera securityCamera;
    private WindowSensor windowSensor;
    
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string DeviceDescription = "This is a device";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const long DeviceModel = 1345354616346;
    private const bool DeviceIsConnected = true;
    
    
    [TestInitialize]
    public void TestInitialize()
    {
        securityCamera = new SecurityCamera();
        windowSensor = new WindowSensor();
    }
    
    [TestMethod]
    public void TestConnectSecurityCamera()
    {
        securityCamera.IsConnected = DeviceIsConnected;
        
        Assert.IsTrue(securityCamera.IsConnected);
    }
    
    [TestMethod]
    public void TestAddNameToSecurityCamera()
    {
        securityCamera.Name = CameraName;
        
        Assert.AreEqual(CameraName, securityCamera.Name);
    }
    
    [TestMethod]
    public void TestAddModelToSecurityCamera()
    {
        securityCamera.Model = DeviceModel;
        
        Assert.AreEqual(DeviceModel, securityCamera.Model);
    }
    
    [TestMethod]
    public void TestAddPhotosToSecurityCamera()
    {
        List<string> photos = new List<string> { DevicePhotoUrl, DevicePhotoUrl };
        
        securityCamera.PhotoURLs = photos;
        
        Assert.AreEqual(photos, securityCamera.PhotoURLs);
    }

    [TestMethod]
    public void TestAddCompanyDataToSecurityCamera()
    {
        Company company = new Company(){};
        
        securityCamera.Company = company;
        
        Assert.AreEqual(company, securityCamera.Company);
    }
    
    [TestMethod]
    public void TestAddDescriptionToSecurityCamera()
    {
        securityCamera.Description = DeviceDescription;
        
        Assert.AreEqual(DeviceDescription, securityCamera.Description);
    }
    
    [TestMethod]
    public void TestAddIndoorLocationTypeToSecurityCamera()
    {
        LocationType locationType = LocationType.Indoor;
        
        securityCamera.LocationType = locationType;
        
        Assert.AreEqual(locationType, securityCamera.LocationType);
    }

    [TestMethod]
    public void TestAddOutdoorLocationTypeToSecurityCamera()
    {
        LocationType locationType = LocationType.Outdoor;
        
        securityCamera.LocationType = locationType;
        
        Assert.AreEqual(locationType, securityCamera.LocationType);
    }
    
    [TestMethod]
    public void TestAddMovementDetectionFunctionalityToSecurityCamera()
    {
        List<SecurityCameraFunctionality> functionalities = new List<SecurityCameraFunctionality> {SecurityCameraFunctionality.MotionDetection};
        
        securityCamera.Functionalities = functionalities;
        
        Assert.AreEqual(functionalities, securityCamera.Functionalities);
    }
    
    [TestMethod]
    public void TestAddHumanDetectionFunctionalityToSecurityCamera()
    {
        List<SecurityCameraFunctionality> functionalities = new List<SecurityCameraFunctionality> {SecurityCameraFunctionality.HumanDetection};
        
        securityCamera.Functionalities = functionalities;
        
        Assert.AreEqual(functionalities, securityCamera.Functionalities);
    }
    
    [TestMethod]
    public void TestAddOpenClosedFunctionalityToWindowSensor()
    {
        List<WindowSensorFunctionality> functionalities = new List<WindowSensorFunctionality> {WindowSensorFunctionality.OpenClosed};
        
        windowSensor.Functionalities = functionalities;
        
        Assert.AreEqual(windowSensor.Functionalities, functionalities);
    }
    
    [TestMethod]
    public void TestAddDeviceCharacteristicsToWindowSensor()
    {
        windowSensor = new WindowSensor() 
        {
            Name = WindowSensorName,
        };
        
        Assert.IsNotNull(windowSensor);
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
            Company = company,
            IsConnected = DeviceIsConnected
        };
        
        WindowSensor windowSensor2 = new WindowSensor()
        {
            Id = 2,
            Name = WindowSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = functionalities,
            Company = company,
            IsConnected = DeviceIsConnected
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
            Company = company,
            IsConnected = DeviceIsConnected
        };
        
        SecurityCamera securityCamera2 = new SecurityCamera()
        {
            Id = 2,
            Name = CameraName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = functionalities,
            Company = company,
            IsConnected = DeviceIsConnected
        };
        
        Assert.IsFalse(securityCamera1.Equals(securityCamera2));
    }
}