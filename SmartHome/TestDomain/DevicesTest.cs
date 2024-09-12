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
        // Arrange
        SecurityCamera device = new SecurityCamera();

        // Act
        device.IsConnected = true;

        // Assert
        Assert.IsTrue(device.IsConnected);
    }
    
    [TestMethod]
    public void TestAddNameToSecurityCamera()
    {
        // Arrange
        SecurityCamera device = new SecurityCamera();

        // Act
        device.Name = "Camera 1";

        // Assert
        Assert.AreEqual("Camera 1", device.Name);
    }
    
    [TestMethod]
    public void TestAddModelToSecurityCamera()
    {
        // Arrange
        SecurityCamera device = new SecurityCamera();

        // Act
        device.Model = 1345354616346;

        // Assert
        Assert.AreEqual(1345354616346, device.Model);
    }
    
    [TestMethod]
    public void TestAddPhotosToSecurityCamera()
    {
        // Arrange
        SecurityCamera device = new SecurityCamera();
        List<string> photos = new List<string> { "https://example.com/photo1.jpg", "https://example.com/photo2.jpg" };

        // Act
        device.PhotoURLs = photos;

        // Assert
        Assert.AreEqual(photos, device.PhotoURLs);
    }

    [TestMethod]
    public void TestAddCompanyDataToSecurityCamera()
    {
        // Arrange
        SecurityCamera device = new SecurityCamera();
        
        // Act
        device.CompanyName = "SecurityCameras & Co.";
        device.CompanyRUT = "123456789";
        device.CompanyLogoURL = "https://example.com/logo.jpg";
        
        // Assert
        Assert.AreEqual("SecurityCameras & Co.", device.CompanyName);
        Assert.AreEqual("123456789", device.CompanyRUT);
        Assert.AreEqual("https://example.com/logo.jpg", device.CompanyLogoURL);
    }
}