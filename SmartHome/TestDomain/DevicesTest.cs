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
        device.Connect();

        // Assert
        Assert.IsTrue(device.IsConnected);
    }
}