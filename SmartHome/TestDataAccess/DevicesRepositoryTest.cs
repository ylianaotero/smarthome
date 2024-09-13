using DataAccess;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using IDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        Assert.IsTrue(devices.TrueForAll(device => device.Model == model));
    }
}