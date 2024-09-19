using BusinessLogic.IServices;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Out;

namespace TestWebApi;

[TestClass]
public class DevicesControllerTest
{
    private DeviceController _deviceController;
    private Mock<IDeviceService> _mockIDeviceService;
    private SecurityCamera _defaultCamera;
    private WindowSensor _defaultWindowSensor;
    private Company _defaultCompany;
    
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const long DeviceModel = 1345354616346;
    
    private const string CompanyName = "IoT Devices & Co.";
    
    [TestInitialize]
    public void TestInitialize()
    {
        SetupDeviceController();
        SetupDefaultObjects();
    }
    
    [TestMethod]
    public void TestGetAllDevicesOkStatusCode()
    {
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        devices.Add(_defaultWindowSensor);
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        
        ObjectResult result = _deviceController.GetDevices(null, null, null, null, null) as OkObjectResult;
        
        Assert.AreEqual(200, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetAllDevicesUnauthorizedStatusCode()
    {
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        devices.Add(_defaultWindowSensor);
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        
        ObjectResult result = _deviceController.GetDevices(null, null, null, null, null) as UnauthorizedObjectResult;
        
        Assert.AreEqual(401, result.StatusCode);
    }

    [TestMethod]
    public void TestGetAllDevicesOkResponse()
    {
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        devices.Add(_defaultWindowSensor);
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        DeviceResponse device1Response = new DeviceResponse()
        {
            Name = CameraName,
            Model = DeviceModel,
            PhotoUrl = DevicePhotoUrl,
            CompanyName = CompanyName,
        };

        DeviceResponse device2Response = new DeviceResponse()
        {
            Name = WindowSensorName,
            Model = DeviceModel,
            PhotoUrl = DevicePhotoUrl,
            CompanyName = CompanyName,
        };

        DevicesResponse expectedResponse = new DevicesResponse()
        {
            Devices = new List<DeviceResponse>()
            {
                device1Response,
                device2Response,
            },
        };
        
        ObjectResult result = _deviceController.GetDevices(null, null, null, null, null) as OkObjectResult;
        DevicesResponse response = result.Value as DevicesResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDeviceTypesOkStatusCode()
    {
        List<string> deviceTypes = new List<string>()
        {
            "SecurityCamera",
            "WindowSensor"
        };

        _mockIDeviceService.Setup(service => service.GetDeviceTypes()).Returns(deviceTypes);
        
        ObjectResult result = _deviceController.GetDeviceTypes() as OkObjectResult;
        
        Assert.AreEqual(200, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDeviceTypesOkResponse()
    {
        List<string> deviceTypes = new List<string>()
        {
            "SecurityCamera",
            "WindowSensor"
        };
        _mockIDeviceService.Setup(service => service.GetDeviceTypes()).Returns(deviceTypes);
        
        DeviceTypesResponse expectedResponse = new DeviceTypesResponse()
        {
            DeviceTypes = deviceTypes,
        };
        
        ObjectResult result = _deviceController.GetDeviceTypes() as OkObjectResult;
        DeviceTypesResponse response = result.Value as DeviceTypesResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDevicesFilteredByOkStatusCode()
    {
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);

        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        
        ObjectResult result = _deviceController.GetDevices(null, CameraName, DeviceModel.ToString(), CompanyName, "SecurityCamera") as OkObjectResult;
        
        Assert.AreEqual(200, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDevicesFilteredByOkStatusResponse()
    {
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);

        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        DeviceResponse device1Response = new DeviceResponse()
        {
            Name = CameraName,
            Model = DeviceModel,
            PhotoUrl = DevicePhotoUrl,
            CompanyName = CompanyName,
        };
        
        DevicesResponse expectedResponse = new DevicesResponse()
        {
            Devices = new List<DeviceResponse>()
            {
                device1Response,
            },
        };
        
        ObjectResult result = _deviceController.GetDevices(null, CameraName, DeviceModel.ToString(), CompanyName, "SecurityCamera") as OkObjectResult;
        DevicesResponse response = result.Value as DevicesResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    private void SetupDefaultObjects()
    {
        List<string> photos = new List<string>()
        {
            DevicePhotoUrl,
        };
        
        _defaultCompany = new Company { Name = CompanyName };

        _defaultCamera = new SecurityCamera()
            { Name = CameraName, Model = DeviceModel, PhotoURLs = photos, Company = _defaultCompany, Type = "SecurityCamera" };
        _defaultWindowSensor = new WindowSensor()
            { Name = WindowSensorName, Model = DeviceModel, PhotoURLs = photos, Company = _defaultCompany, Type = "WindowSensor" };
    }

    private void SetupDeviceController()
    {
        _mockIDeviceService = new Mock<IDeviceService>();
        _deviceController = new DeviceController(_mockIDeviceService.Object);
    }
}
