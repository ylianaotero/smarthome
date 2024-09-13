using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using IDomain;

namespace TestDataAccess;

[TestClass]
public class DevicesRepositoryTest
{
    [TestMethod]
    public void TestGetDevicesByModel()
    {
        // Arrange
        DevicesRepository devicesRepository = new DevicesRepository();
        long model = 1345354616346;
        
        // Act
        List<IDevice> devices = devicesRepository.GetDevicesByModel(model);
        
        // Assert
        Assert.IsTrue(devices.All(device => device.Model == model));
    }
}