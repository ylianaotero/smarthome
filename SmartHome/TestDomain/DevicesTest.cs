using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;


namespace TestDomain;

[TestClass]
public class DevicesTest
{
    [TestMethod]
    public void TestConnectSecurityCamera()
    {
        SecurityCamera device = new SecurityCamera();
        
        device.IsConnected = true;
        
        Assert.IsTrue(device.IsConnected);
    }
    
    [TestMethod]
    public void TestAddNameToSecurityCamera()
    {
        SecurityCamera device = new SecurityCamera();
        
        device.Name = "Camera 1";
        
        Assert.AreEqual("Camera 1", device.Name);
    }
    
    [TestMethod]
    public void TestAddModelToSecurityCamera()
    {
        SecurityCamera device = new SecurityCamera();
        
        device.Model = 1345354616346;
        
        Assert.AreEqual(1345354616346, device.Model);
    }
    
    [TestMethod]
    public void TestAddPhotosToSecurityCamera()
    {
        SecurityCamera device = new SecurityCamera();
        List<string> photos = new List<string> { "https://example.com/photo1.jpg", "https://example.com/photo2.jpg" };
        
        device.PhotoURLs = photos;
        
        Assert.AreEqual(photos, device.PhotoURLs);
    }

    [TestMethod]
    public void TestAddCompanyDataToSecurityCamera()
    {
        SecurityCamera device = new SecurityCamera();
        Company company = new Company()
        {
            Name = "SecurityCameras & Co.",
            RUT = "123456789",
            LogoURL = "https://example.com/logo.jpg"
        };
        
        device.Company = company;
        
        Assert.AreEqual(company, device.Company);
        Assert.AreEqual("SecurityCameras & Co.", device.Company.Name);
        Assert.AreEqual("123456789", device.Company.RUT);
        Assert.AreEqual("https://example.com/logo.jpg", device.Company.LogoURL);
    }
    
    [TestMethod]
    public void TestAddDescriptionToSecurityCamera()
    {
        SecurityCamera device = new SecurityCamera();
        string description = "This is a security camera";
        
        device.Description = description;
        
        Assert.AreEqual(description, device.Description);
    }
    
}