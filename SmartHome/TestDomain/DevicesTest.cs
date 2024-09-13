using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;


namespace TestDomain;

[TestClass]
public class DevicesTest
{
    private SecurityCamera securityCamera;
    private WindowSensor windowSensor;
    
    [TestInitialize]
    public void TestInitialize()
    {
        securityCamera = new SecurityCamera();
        windowSensor = new WindowSensor();
    }
    
    [TestMethod]
    public void TestConnectSecurityCamera()
    {
        securityCamera.IsConnected = true;
        
        Assert.IsTrue(securityCamera.IsConnected);
    }
    
    [TestMethod]
    public void TestAddNameToSecurityCamera()
    {
        securityCamera.Name = "Camera 1";
        
        Assert.AreEqual("Camera 1", securityCamera.Name);
    }
    
    [TestMethod]
    public void TestAddModelToSecurityCamera()
    {
        securityCamera.Model = 1345354616346;
        
        Assert.AreEqual(1345354616346, securityCamera.Model);
    }
    
    [TestMethod]
    public void TestAddPhotosToSecurityCamera()
    {
        List<string> photos = new List<string> { "https://example.com/photo1.jpg", "https://example.com/photo2.jpg" };
        
        securityCamera.PhotoURLs = photos;
        
        Assert.AreEqual(photos, securityCamera.PhotoURLs);
    }

    [TestMethod]
    public void TestAddCompanyDataToSecurityCamera()
    {
        Company company = new Company()
        {
            Name = "SecurityCameras & Co.",
            RUT = "123456789",
            LogoURL = "https://example.com/logo.jpg"
        };
        
        securityCamera.Company = company;
        
        Assert.IsTrue(company.Equals(securityCamera.Company));
    }
    
    [TestMethod]
    public void TestAddDescriptionToSecurityCamera()
    {
        string description = "This is a security camera";
        
        securityCamera.Description = description;
        
        Assert.AreEqual(description, securityCamera.Description);
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
            Name = "Window Sensor 1",
            Model = 1345354616346,
            Description = "This is a window sensor",
            PhotoURLs = new List<string> { "https://example.com/photo1.jpg"},
            Company = new Company() {},
            IsConnected = true
        };
        
        Assert.IsNotNull(windowSensor);
    }

    [TestMethod]
    public void TestDifferentiationOfWindowSensorsViaHardwardId()
    {
        WindowSensor windowSensor1 = new WindowSensor()
        {
            Id = 1,
            Name = "Window Sensor",
            Model = 1345354616346,
            Description = "This is a window sensor",
            PhotoURLs = new List<string> { "https://example.com/photo1.jpg"},
            Company = new Company() {},
            IsConnected = true
        };
        
        WindowSensor windowSensor2 = new WindowSensor()
        {
            Id = 2,
            Name = "Window Sensor",
            Model = 1345354616346,
            Description = "This is a window sensor",
            PhotoURLs = new List<string> { "https://example.com/photo1.jpg"},
            Company = new Company() {},
            IsConnected = true
        };
        
        Assert.IsFalse(windowSensor1.Equals(windowSensor2));
    }
}