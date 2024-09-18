using Domain;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class DeviceServiceTest
{
    [TestMethod]
    public void TestGetAllDevices()
    {
        // Arrange
        List<Device> devices = new List<Device>();
        List<string> photos = new List<string>()
        {
            "https://example.com/photo.jpg",
        };
        Company company = new Company { Name = "My Company" };

        SecurityCamera device1 = new SecurityCamera()
            { Name = "My Security Camera", Model = 1345354616346, PhotoURLs = photos, Company = company };
        WindowSensor device2 = new WindowSensor()
            { Name = "My Window Sensor", Model = 1345354616346, PhotoURLs = photos, Company = company };
        devices.Add(device1);
        devices.Add(device2);
        
        var mockDeviceRepository = new Mock<IRepository<Device>>();
        
        mockDeviceRepository.Setup(x => x.GetAll()).Returns(devices);
        var deviceService = new DeviceService(mockDeviceRepository.Object);
        
        //Act
        List<Device> retrievedDevices =  deviceService.GetAllDevices();
        
        // Assert
        Assert.AreEqual(devices, retrievedDevices);

    }
}