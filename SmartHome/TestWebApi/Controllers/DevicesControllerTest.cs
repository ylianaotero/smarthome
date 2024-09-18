using BusinessLogic.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestWebApi;

[TestClass]
public class DevicesControllerTest
{
    [TestMethod]
    public void TestGetAllDevicesOkStatusCode()
    {
        //Arrange
        Mock<DeviceService> deviceServiceMock = new Mock<DeviceService>();
        DeviceController deviceController = new DeviceController(deviceServiceMock.Object);
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

        deviceServiceMock.Setup(service => service.GetAllDevices()).Returns(devices);

        //Act
        var result = deviceController.GetDevices() as OkObjectResult;

        //Assert
        Assert.AreEqual(200, result.StatusCode);
    }
}
