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
        var device = new SecurityCamera();

        // Act
        device.IsConnected = true;

        // Assert
        Assert.IsTrue(device.IsConnected);
    }
    
    [TestMethod]
    public void TestAddNameToSecurityCamera()
    {
        // Arrange
        var device = new SecurityCamera();

        // Act
        device.Name = "Camera 1";

        // Assert
        Assert.AreEqual("Camera 1", device.Name);
    }
}