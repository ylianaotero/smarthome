using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;


namespace TestDomain;

[TestClass]
public class DevicesTest
{
    private SecurityCamera device;
    
    [TestInitialize]
    public void TestInitialize()
    {
        device = new SecurityCamera();
    }
    
    [TestMethod]
    public void TestConnectSecurityCamera()
    {
        device.IsConnected = true;
        
        Assert.IsTrue(device.IsConnected);
    }
    
    [TestMethod]
    public void TestAddNameToSecurityCamera()
    {
        device.Name = "Camera 1";
        
        Assert.AreEqual("Camera 1", device.Name);
    }
    
    [TestMethod]
    public void TestAddModelToSecurityCamera()
    {
        device.Model = 1345354616346;
        
        Assert.AreEqual(1345354616346, device.Model);
    }
    
    [TestMethod]
    public void TestAddPhotosToSecurityCamera()
    {
        List<string> photos = new List<string> { "https://example.com/photo1.jpg", "https://example.com/photo2.jpg" };
        
        device.PhotoURLs = photos;
        
        Assert.AreEqual(photos, device.PhotoURLs);
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
        
        device.Company = company;
        
        Assert.IsTrue(company.Equals(device.Company));
    }
    
    [TestMethod]
    public void TestAddDescriptionToSecurityCamera()
    {
        string description = "This is a security camera";
        
        device.Description = description;
        
        Assert.AreEqual(description, device.Description);
    }
    
    [TestMethod]
    public void TestAddIndoorLocationTypeToSecurityCamera()
    {
        LocationType locationType = LocationType.Indoor;
        
        device.LocationType = locationType;
        
        Assert.AreEqual(locationType, device.LocationType);
    }

    [TestMethod]
    public void TestAddOutdoorLocationTypeToSecurityCamera()
    {
        LocationType locationType = LocationType.Outdoor;
        
        device.LocationType = locationType;
        
        Assert.AreEqual(locationType, device.LocationType);
    }
    
    [TestMethod]
    public void TestAddMovementDetectionFunctionalityToSecurityCamera()
    {
        List<Functionality> functionalities = new List<Functionality> {Functionality.MotionDetection};
        
        device.Functionalities = functionalities;
        
        Assert.AreEqual(functionalities, device.Functionalities);
    }
    
    [TestMethod]
    public void TestAddHumanDetectionFunctionalityToSecurityCamera()
    {
        List<Functionality> functionalities = new List<Functionality> {Functionality.HumanDetection};
        
        device.Functionalities = functionalities;
        
        Assert.AreEqual(functionalities, device.Functionalities);
    }
}