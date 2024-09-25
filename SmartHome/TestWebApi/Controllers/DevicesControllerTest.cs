using IBusinessLogic;
using DataAccess.Exceptions;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.Out;
using WebApi.Models.In;

namespace TestWebApi.Controllers;

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
    private const string SecurityCameraType = "SecurityCamera";
    private const string WindowSensorType = "WindowSensor";
    private const string SessionDoesNotExistExceptionMessage = "User not found";
    private const string DeviceNotFoundExceptionMessage = "Device not found";
    private const int OkStatusCode = 200;
    private const int CreatedStatusCode = 201;
    private const int UnauthorizedStatusCode = 401;
    
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
        List<Device> devices =
        [
            _defaultCamera,
            _defaultWindowSensor
        ];
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetDevicesByFilter(It.IsAny<Func<Device, bool>>())).Returns(devices);
        
        DeviceRequest request = new DeviceRequest();
        
        ObjectResult? result = _deviceController.GetDevices(token, request) as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetAllDevicesWhenThereAreNoDevicesOkStatusCode()
    {
        Guid token = Guid.NewGuid();
        List<Device> devices = [];
        
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetDevicesByFilter(It.IsAny<Func<Device, bool>>())).Returns(devices);

        DeviceRequest request = new DeviceRequest();
        
        ObjectResult? result = _deviceController.GetDevices(token, request) as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetAllDevicesUnauthorizedStatusCode()
    {
        List<Device> devices =
        [
            _defaultCamera,
            _defaultWindowSensor
        ];
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        
        DeviceRequest request = new DeviceRequest();
        
        ObjectResult? result = _deviceController.GetDevices(null, request) as UnauthorizedObjectResult;
        
        Assert.AreEqual(UnauthorizedStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetAllDevicesUnauthorizedWithTokenStatusCode()
    {
        Guid token = Guid.NewGuid();
        List<Device> devices =
        [
            _defaultCamera,
            _defaultWindowSensor
        ];
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>()))
            .Throws(new CannotFindItemInList(SessionDoesNotExistExceptionMessage));
        _mockIDeviceService.Setup(service => service.GetAllDevices()).Returns(devices);
        
        DeviceRequest request = new DeviceRequest();
        
        ObjectResult? result = _deviceController.GetDevices(token, request) as UnauthorizedObjectResult;
        
        Assert.AreEqual(UnauthorizedStatusCode, result.StatusCode);
    }

    [TestMethod]
    public void TestGetAllDevicesOkResponse()
    {
        Guid token = Guid.NewGuid();
        List<Device> devices =
        [
            _defaultCamera,
            _defaultWindowSensor
        ];
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetDevicesByFilter(It.IsAny<Func<Device, bool>>())).Returns(devices);
        DevicesResponse expectedResponse = DefaultDevicesResponse();

        DeviceRequest request = new DeviceRequest();
        
        ObjectResult? result = _deviceController.GetDevices(token, request) as OkObjectResult;
        DevicesResponse response = (result!.Value as DevicesResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }
   
    [TestMethod]
    public void TestGetDeviceByIdOkStatusCode()
    {
        Guid token = Guid.NewGuid();
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetDeviceById(1)).Returns(_defaultCamera);
        
        ObjectResult? result = _deviceController.GetDeviceById(token, 1) 
            as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDeviceByIdOkResponse()
    {
        Guid token = Guid.NewGuid();
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetDeviceById(1)).Returns(_defaultCamera);
        DeviceResponse expectedResponse = DefaultSecurityCameraResponse();
        
        ObjectResult? result = _deviceController.GetDeviceById(token, 1) as OkObjectResult;
        DeviceResponse response = (result!.Value as DeviceResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDeviceByIdUnauthorizedStatusCode()
    {
        _mockIDeviceService.Setup(service => service.GetDeviceById(1)).Returns(_defaultCamera);
        
        ObjectResult? result = _deviceController.GetDeviceById(null, 1) as UnauthorizedObjectResult;
        
        Assert.AreEqual(UnauthorizedStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDeviceByIdNotFoundStatusCode()
    {
        Guid token = Guid.NewGuid();
        _mockIDeviceService.Setup(service => service.GetDeviceById(1))
            .Throws(new ElementNotFoundException(DeviceNotFoundExceptionMessage));
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
     
        IActionResult result = _deviceController.GetDeviceById(token, 1);

        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }
    
    [TestMethod]
    public void TestGetDeviceTypesOkStatusCode()
    {
        Guid token = Guid.NewGuid();
        List<string> deviceTypes =
        [
            SecurityCameraType,
            WindowSensorType
        ];
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetDeviceTypes()).Returns(deviceTypes);
        
        ObjectResult? result = _deviceController.GetDeviceTypes(token) as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDeviceTypesOkResponse()
    {
        Guid token = Guid.NewGuid();
        List<string> deviceTypes =
        [
            SecurityCameraType,
            WindowSensorType
        ];
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetDeviceTypes()).Returns(deviceTypes);
        DeviceTypesResponse expectedResponse = new DeviceTypesResponse()
        {
            DeviceTypes = deviceTypes,
        };
        
        ObjectResult? result = _deviceController.GetDeviceTypes(token) as OkObjectResult;
        DeviceTypesResponse response = (result!.Value as DeviceTypesResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDeviceTypesUnauthorizedStatusCode()
    {
        ObjectResult? result = _deviceController.GetDeviceTypes(null) as UnauthorizedObjectResult;
        
        Assert.AreEqual(UnauthorizedStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDevicesFilteredByOkStatusCode()
    {
        Guid token = Guid.NewGuid();
        List<Device> devices =
        [
            _defaultCamera
        ];
        _mockIDeviceService.Setup(service => service.GetDevicesByFilter(It.IsAny<Func<Device, bool>>())).Returns(devices);
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        DeviceRequest request = new DeviceRequest()
        {
            Name = CameraName,
            Model = DeviceModel,
            Company = CompanyName,
            Kind = SecurityCameraType,
        };
        
        ObjectResult? result = _deviceController.GetDevices(token, request) as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDevicesFilteredByOkStatusResponse()
    {
        Guid token = Guid.NewGuid();
        DeviceRequest request = new DeviceRequest()
        {
            Name = CameraName,
            Model = DeviceModel,
            Company = CompanyName,
            Kind = SecurityCameraType,
        };
        List<Device> devices =
        [
            new SecurityCamera()
            {
                Name = request.Name,
                Model = request.Model!.Value,
                PhotoURLs = new List<string>() { DevicePhotoUrl },
                Company = new Company()
                {
                    Name = request.Company
                },
                Kind = request.Kind!,
            }
        ];
        _mockIDeviceService.Setup(service => service.GetDevicesByFilter(It.IsAny<Func<Device, bool>>())).Returns(devices);
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        DevicesResponse? expectedResponse = new DevicesResponse(devices);
        
        ObjectResult? result = _deviceController.GetDevices(token, request) as OkObjectResult;
        DevicesResponse? response = result!.Value as DevicesResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestPostWindowSensorsCreatedStatusCode()
    {
        Guid token = Guid.NewGuid();
        WindowSensorRequest request = DefaultWindowSensorRequest();
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(DefaultCompanyOwner());
        _mockIDeviceService.Setup(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company == request.Company &&
            ((device as WindowSensor)!).Functionalities!.SequenceEqual(request.Functionalities!)
        )));
        
        ObjectResult? result = _deviceController.PostWindowSensors(token, request) as CreatedResult;
        
        _mockIDeviceService.Verify(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company == request.Company &&
            ((device as WindowSensor)!).Functionalities!.SequenceEqual(request.Functionalities!)
        )), Times.Once);
        Assert.AreEqual(CreatedStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostWindowSensorsUnauthorizedStatusCode()
    {
        WindowSensorRequest request = DefaultWindowSensorRequest();

        ObjectResult? result = _deviceController.PostWindowSensors(null, request) as UnauthorizedObjectResult;
        
        Assert.AreEqual(UnauthorizedStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostWindowSensorsForbiddenStatusCode()
    {
        Guid token = Guid.NewGuid();
        WindowSensorRequest request = DefaultWindowSensorRequest();
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());

        IActionResult result = _deviceController.PostWindowSensors(token, request);

        Assert.IsInstanceOfType(result, typeof(ForbidResult));
    }
    
    [TestMethod]
    public void TestPostSecurityCamerasCreatedStatusCode()
    {
        Guid token = Guid.NewGuid();
        SecurityCameraRequest request = DefaultSecurityCameraRequest();
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(DefaultCompanyOwner());
        _mockIDeviceService.Setup(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company == request.Company &&
            ((device as SecurityCamera)!).LocationType == request.LocationType &&
            ((device as SecurityCamera)!).Functionalities != null &&
            ((device as SecurityCamera)!).Functionalities!.SequenceEqual(request.Functionalities)
        )));
        
        ObjectResult? result = _deviceController.PostSecurityCameras(token, request) as CreatedResult;
        
        _mockIDeviceService.Verify(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company == request.Company &&
            ((device as SecurityCamera)!).LocationType == request.LocationType &&
            ((device as SecurityCamera)!).Functionalities != null &&
            ((device as SecurityCamera)!).Functionalities!.SequenceEqual(request.Functionalities)
        )), Times.Once);
        Assert.AreEqual(CreatedStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostSecurityCamerasUnauthorizedStatusCode()
    {
        SecurityCameraRequest request = DefaultSecurityCameraRequest();

        ObjectResult? result = _deviceController.PostSecurityCameras(null, request) as UnauthorizedObjectResult;
        
        Assert.AreEqual(UnauthorizedStatusCode, result!.StatusCode);
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
        List<string> photos =
        [
            DevicePhotoUrl
        ];
        
        _defaultCompany = new Company { Name = CompanyName };

        _defaultCamera = new SecurityCamera()
            { Name = CameraName, Model = DeviceModel, PhotoURLs = photos, Company = _defaultCompany, Kind = SecurityCameraType };
        _defaultWindowSensor = new WindowSensor()
            { Name = WindowSensorName, Model = DeviceModel, PhotoURLs = photos, Company = _defaultCompany, Kind = WindowSensorType };
    }

    private void SetupDeviceController()
    {
        _mockIDeviceService = new Mock<IDeviceService>();
        _mockISessionService = new Mock<ISessionService>();
        _deviceController = new DeviceController(_mockIDeviceService.Object, _mockISessionService.Object);
    }
    
    private WindowSensorRequest DefaultWindowSensorRequest()
    {
        return new WindowSensorRequest()
        {
            Name = WindowSensorName,
            Model = DeviceModel,
            PhotoUrls = [DevicePhotoUrl],
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
            PhotoUrls = [DevicePhotoUrl],
            Description = DeviceDescription,
            Company = _defaultCompany,
            LocationType = LocationType.Indoor,
            Functionalities = [SecurityCameraFunctionality.MotionDetection],
        };
    }
    
    private DevicesResponse DefaultDevicesResponse()
    {
        List<Device> devices =
        [
            _defaultCamera,
            _defaultWindowSensor
        ];

        return new DevicesResponse(devices);
    }
    
    private DeviceResponse DefaultSecurityCameraResponse()
    {
        return new DeviceResponse(_defaultCamera);
    }

    private User DefaultCompanyOwner()
    {
        return new User()
        {
            Roles = [
                new CompanyOwner()
                {
                    Company = _defaultCompany
                }
            ]
        };
    }
}