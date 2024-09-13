using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDataAccess;

[TestClass]
public class DevicesRepositoryTest
{
    [TestMethod]
    public void TestGetDeviceByModel()
    {
        // Arrange
        DevicesRepository devicesRepository = new DevicesRepository();
        long model = 1345354616346;
        
        // Act
        Device device = devicesRepository.GetDeviceByModel(model);
        
        // Assert
        Assert.AreEqual(device.Model, model);
    }
}