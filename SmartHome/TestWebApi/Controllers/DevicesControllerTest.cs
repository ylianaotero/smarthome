using BusinessLogic.IServices;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.In;
using WebApi.Out;

namespace TestWebApi;

[TestClass]
public class DevicesControllerTest
{
    private DeviceController _deviceController;
    private Mock<IDeviceService> _mockIDeviceService;
    private Mock<ISessionService> _mockISessionService;
    private SecurityCamera _defaultCamera;
    private WindowSensor _defaultWindowSensor;
    private Company _defaultCompany;
    
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const long DeviceModel = 1345354616346;
    private const string DeviceDescription = "This is a device";
    private const string CompanyName = "IoT Devices & Co.";
    private const string SessionDoesNotExistExceptionMessage = "User not found";
    
    [TestInitialize]
    public void TestInitialize()
    {
        SetupDeviceController();
        SetupDefaultObjects();
    }
    
    [TestMethod]
    public void TestGetAllDevicesOkStatusCode()
    {
        Guid token = Guid.NewGuid();
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        devices.Add(_defaultWindowSensor);
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        
        ObjectResult result = _deviceController.GetDevices(token, null, null, null, null) 
            as OkObjectResult;
        
        Assert.AreEqual(200, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetAllDevicesWhenThereAreNoDevicesOkStatusCode()
    {
        Guid token = Guid.NewGuid();
        List<Device> devices = new List<Device>();
        
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        
        ObjectResult result = _deviceController.GetDevices(token, null, null, null, null) 
            as OkObjectResult;
        
        Assert.AreEqual(200, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetAllDevicesUnauthorizedStatusCode()
    {
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        devices.Add(_defaultWindowSensor);
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        
        ObjectResult result = _deviceController.GetDevices(null, null, null, null, null)
            as UnauthorizedObjectResult;
        
        Assert.AreEqual(401, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetAllDevicesUnauthorizedWithTokenStatusCode()
    {
        Guid token = Guid.NewGuid();
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        devices.Add(_defaultWindowSensor);
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>()))
            .Throws(new CannotFindItemInList(SessionDoesNotExistExceptionMessage));
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        
        ObjectResult result = _deviceController.GetDevices(token, null, null, null, null) 
            as UnauthorizedObjectResult;
        
        Assert.AreEqual(401, result.StatusCode);
    }

    [TestMethod]
    public void TestGetAllDevicesOkResponse()
    {
        Guid token = Guid.NewGuid();
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        devices.Add(_defaultWindowSensor);
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        DevicesResponse expectedResponse = DefaultDevicesResponse();
        
        ObjectResult result = _deviceController.GetDevices(token, null, null, null, null) 
            as OkObjectResult;
        DevicesResponse response = result.Value as DevicesResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDeviceTypesOkStatusCode()
    {
        Guid token = Guid.NewGuid();
        List<string> deviceTypes = new List<string>()
        {
            "SecurityCamera",
            "WindowSensor"
        };
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetDeviceTypes()).Returns(deviceTypes);
        
        ObjectResult result = _deviceController.GetDeviceTypes(token) as OkObjectResult;
        
        Assert.AreEqual(200, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDeviceTypesOkResponse()
    {
        Guid token = Guid.NewGuid();
        List<string> deviceTypes = new List<string>()
        {
            "SecurityCamera",
            "WindowSensor"
        };
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetDeviceTypes()).Returns(deviceTypes);
        DeviceTypesResponse expectedResponse = new DeviceTypesResponse()
        {
            DeviceTypes = deviceTypes,
        };
        
        ObjectResult result = _deviceController.GetDeviceTypes(token) as OkObjectResult;
        DeviceTypesResponse response = result.Value as DeviceTypesResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDeviceTypesUnauthorizedStatusCode()
    {
        ObjectResult result = _deviceController.GetDeviceTypes(null) as UnauthorizedObjectResult;
        
        Assert.AreEqual(401, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDevicesFilteredByOkStatusCode()
    {
        Guid token = Guid.NewGuid();
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        
        ObjectResult result = _deviceController.GetDevices(token, CameraName, DeviceModel.ToString(), 
            CompanyName, "SecurityCamera") as OkObjectResult;
        
        Assert.AreEqual(200, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDevicesFilteredByOkStatusResponse()
    {
        Guid token = Guid.NewGuid();
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        DeviceResponse device1Response = DefaultSecurityCameraResponse();
        DevicesResponse expectedResponse = new DevicesResponse()
        {
            Devices = new List<DeviceResponse>()
            {
                device1Response,
            },
        };
        
        ObjectResult result = _deviceController.GetDevices(token, CameraName, DeviceModel.ToString(), 
            CompanyName, "SecurityCamera") as OkObjectResult;
        DevicesResponse response = result.Value as DevicesResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestPostWindowSensorsCreatedStatusCode()
    {
        Guid token = Guid.NewGuid();
        WindowSensorRequest request = DefaultWindowSensorRequest();
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company == request.Company &&
            (device as WindowSensor).Functionalities.SequenceEqual(request.Functionalities)
        )));
        
        ObjectResult result = _deviceController.PostWindowSensors(token, request) as CreatedResult;
        
        _mockIDeviceService.Verify(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company == request.Company &&
            (device as WindowSensor).Functionalities.SequenceEqual(request.Functionalities)
        )), Times.Once);
        Assert.AreEqual(201, result.StatusCode);
    }
    
    [TestMethod]
    public void TestPostWindowSensorsUnauthorizedStatusCode()
    {
        WindowSensorRequest request = DefaultWindowSensorRequest();

        ObjectResult result = _deviceController.PostWindowSensors(null, request) as UnauthorizedObjectResult;
        
        Assert.AreEqual(401, result.StatusCode);
    }
    
    [TestMethod]
    public void TestPostSecurityCamerasCreatedStatusCode()
    {
        Guid token = Guid.NewGuid();
        SecurityCameraRequest request = DefaultSecurityCameraRequest();
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User()
        {
            Roles = new List<Role>()
            {
                new CompanyOwner()
            }
        });
        _mockIDeviceService.Setup(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company == request.Company &&
            (device as SecurityCamera).LocationType == request.LocationType &&
            (device as SecurityCamera).Functionalities != null &&
            (device as SecurityCamera).Functionalities.SequenceEqual(request.Functionalities)
        )));
        
        ObjectResult result = _deviceController.PostSecurityCameras(token, request) as CreatedResult;
        
        _mockIDeviceService.Verify(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company == request.Company &&
            (device as SecurityCamera).LocationType == request.LocationType &&
            (device as SecurityCamera).Functionalities != null &&
            (device as SecurityCamera).Functionalities.SequenceEqual(request.Functionalities)
        )), Times.Once);
        Assert.AreEqual(201, result.StatusCode);
    }
    
    [TestMethod]
    public void TestPostSecurityCamerasUnauthorizedStatusCode()
    {
        SecurityCameraRequest request = DefaultSecurityCameraRequest();

        ObjectResult result = _deviceController.PostSecurityCameras(null, request) as UnauthorizedObjectResult;
        
        Assert.AreEqual(401, result.StatusCode);
    }
    
    [TestMethod]
    public void TestPostSecurityCamerasForbiddenStatusCode()
    {
        Guid token = Guid.NewGuid();
        SecurityCameraRequest request = DefaultSecurityCameraRequest();
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());

        IActionResult result = _deviceController.PostSecurityCameras(token, request);

        Assert.IsInstanceOfType(result, typeof(ForbidResult));
    }
    
    private void SetupDefaultObjects()
    {
        List<string> photos = new List<string>()
        {
            DevicePhotoUrl,
        };
        
        _defaultCompany = new Company { Name = CompanyName };

        _defaultCamera = new SecurityCamera()
            { Name = CameraName, Model = DeviceModel, PhotoURLs = photos, Company = _defaultCompany, Kind = "SecurityCamera" };
        _defaultWindowSensor = new WindowSensor()
            { Name = WindowSensorName, Model = DeviceModel, PhotoURLs = photos, Company = _defaultCompany, Kind = "WindowSensor" };
    }

    private void SetupDeviceController()
    {
        _mockIDeviceService = new Mock<IDeviceService>();
        _mockISessionService = new Mock<ISessionService>();
        _deviceController = new DeviceController(_mockIDeviceService.Object, _mockISessionService.Object);
    }
    
    private DevicesResponse DefaultDevicesResponse()
    {
        return new DevicesResponse()
        {
            Devices = new List<DeviceResponse>()
            {
                DefaultSecurityCameraResponse(),
                DefaultWindowSensorResponse(),
            },
        };
    }
    
    private DeviceResponse DefaultSecurityCameraResponse()
    {
        return new DeviceResponse()
        {
            Name = CameraName,
            Model = DeviceModel,
            PhotoUrl = DevicePhotoUrl,
            CompanyName = CompanyName,
        };
    }
    
    private DeviceResponse DefaultWindowSensorResponse()
    {
        return new DeviceResponse()
        {
            Name = WindowSensorName,
            Model = DeviceModel,
            PhotoUrl = DevicePhotoUrl,
            CompanyName = CompanyName,
        };
    }
    
    private WindowSensorRequest DefaultWindowSensorRequest()
    {
        return new WindowSensorRequest()
        {
            Name = WindowSensorName,
            Model = DeviceModel,
            PhotoUrls = new List<string>() { DevicePhotoUrl },
            Description = DeviceDescription,
            Functionalities = new List<WindowSensorFunctionality>() { WindowSensorFunctionality.OpenClosed },
            Company = _defaultCompany,
        };
    }

    private SecurityCameraRequest DefaultSecurityCameraRequest()
    {
        return new SecurityCameraRequest()
        {
            Name = WindowSensorName,
            Model = DeviceModel,
            PhotoUrls = new List<string>() { DevicePhotoUrl },
            Description = DeviceDescription,
            Company = _defaultCompany,
            LocationType = LocationType.Indoor,
            Functionalities = new List<SecurityCameraFunctionality>() { SecurityCameraFunctionality.MotionDetection },
        };
    }
}
