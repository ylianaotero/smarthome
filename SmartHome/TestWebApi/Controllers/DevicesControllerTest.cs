using CustomExceptions;
using IBusinessLogic;
using Domain;
using IDataAccess;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using Moq;
using WebApi.Controllers;

namespace TestWebApi.Controllers;

[TestClass]
public class DevicesControllerTest
{
    private DeviceController _deviceController;
    private Mock<IDeviceService> _mockIDeviceService;
    private Mock<ICompanyService> _mockICompanyService;
    private Mock<ISessionService> _mockISessionService;
    private SecurityCamera _defaultCamera;
    private WindowSensor _defaultWindowSensor;
    private Company _defaultCompany;
    
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const long DeviceModel = 1345354616346;
    private const string DeviceDescription = "This is a device";
    private const string CompanyName = "IoT DeviceUnits & Co.";
    private const string SecurityCameraType = "SecurityCamera";
    private const string WindowSensorType = "WindowSensor";
    private const string DeviceNotFoundExceptionMessage = "Device not found";
    private const int OkStatusCode = 200;
    private const int CreatedStatusCode = 201;
    
    [TestInitialize]
    public void TestInitialize()
    {
        SetupDeviceController();
        SetupDefaultObjects();
    }
    
    [TestMethod]
    public void TestGetAllDevicesOkStatusCode()
    {
        List<Device> devices =
        [
            _defaultCamera,
            _defaultWindowSensor
        ];
        _mockIDeviceService
            .Setup(service => service
                .GetDevicesByFilter(It.IsAny<Func<Device, bool>>(), It.IsAny<PageData>()))
            .Returns(devices);
        
        DeviceRequest request = new DeviceRequest();
        
        ObjectResult? result = _deviceController.GetDevices(request, DefaultPageDataRequest()) as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetAllDevicesWhenThereAreNoDevicesOkStatusCode()
    {
        List<Device> devices = [];
        
        _mockIDeviceService
            .Setup(service => service
                .GetDevicesByFilter(It.IsAny<Func<Device, bool>>(), It.IsAny<PageData>()))
            .Returns(devices);

        DeviceRequest request = new DeviceRequest();
        
        ObjectResult? result = _deviceController.GetDevices(request, DefaultPageDataRequest()) as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }

    [TestMethod]
    public void TestGetAllDevicesOkResponse()
    {
        List<Device> devices =
        [
            _defaultCamera,
            _defaultWindowSensor
        ];
        _mockIDeviceService
            .Setup(service => service
                .GetDevicesByFilter(It.IsAny<Func<Device, bool>>(), It.IsAny<PageData>()))
            .Returns(devices);
        DevicesResponse expectedResponse = DefaultDevicesResponse();

        DeviceRequest request = new DeviceRequest();
        
        ObjectResult? result = _deviceController.GetDevices(request, DefaultPageDataRequest()) as OkObjectResult;
        DevicesResponse response = (result!.Value as DevicesResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }
   
    [TestMethod]
    public void TestGetDeviceByIdOkStatusCode()
    {
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockIDeviceService.Setup(service => service.GetDeviceById(1)).Returns(_defaultCamera);
        
        ObjectResult? result = _deviceController.GetDeviceById(1) 
            as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDeviceByIdOkResponse()
    {
        _mockIDeviceService.Setup(service => service.GetDeviceById(1)).Returns(_defaultCamera);
        DeviceResponse expectedResponse = DefaultSecurityCameraResponse();
        
        ObjectResult? result = _deviceController.GetDeviceById(1) as OkObjectResult;
        DeviceResponse response = (result!.Value as DeviceResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDeviceByIdNotFoundStatusCode()
    {
        Guid token = Guid.NewGuid();
        _mockIDeviceService.Setup(service => service.GetDeviceById(1))
            .Throws(new ElementNotFound(DeviceNotFoundExceptionMessage));
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
     
        IActionResult result = _deviceController.GetDeviceById(1);

        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }
    
    [TestMethod]
    public void TestGetDeviceTypesOkStatusCode()
    {
        List<string> deviceTypes =
        [
            SecurityCameraType,
            WindowSensorType
        ];
        _mockIDeviceService.Setup(service => service.GetDeviceTypes()).Returns(deviceTypes);
        
        ObjectResult? result = _deviceController.GetDeviceTypes() as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDeviceTypesOkResponse()
    {
        List<string> deviceTypes =
        [
            SecurityCameraType,
            WindowSensorType
        ];
        _mockIDeviceService.Setup(service => service.GetDeviceTypes()).Returns(deviceTypes);
        DeviceTypesResponse expectedResponse = new DeviceTypesResponse()
        {
            DeviceTypes = deviceTypes,
        };
        
        ObjectResult? result = _deviceController.GetDeviceTypes() as OkObjectResult;
        DeviceTypesResponse response = (result!.Value as DeviceTypesResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDevicesFilteredByOkStatusCode()
    {
        List<Device> devices =
        [
            _defaultCamera
        ];
        _mockIDeviceService
            .Setup(service => service
                .GetDevicesByFilter(It.IsAny<Func<Device, bool>>(), It.IsAny<PageData>()))
            .Returns(devices);
        DeviceRequest request = new DeviceRequest()
        {
            Name = CameraName,
            Model = DeviceModel,
            Company = CompanyName,
            Kind = SecurityCameraType,
        };
        
        ObjectResult? result = _deviceController.GetDevices(request, DefaultPageDataRequest()) as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDevicesFilteredByOkStatusResponse()
    {
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
        _mockIDeviceService
            .Setup(service => service
                .GetDevicesByFilter(It.IsAny<Func<Device, bool>>(), It.IsAny<PageData>()))
            .Returns(devices);
        DevicesResponse? expectedResponse = new DevicesResponse(devices);
        
        ObjectResult? result = _deviceController.GetDevices(request, DefaultPageDataRequest()) as OkObjectResult;
        DevicesResponse? response = result!.Value as DevicesResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestPostWindowSensorsCreatedStatusCode()
    {
        WindowSensorRequest request = DefaultWindowSensorRequest();
        _mockIDeviceService.Setup(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company == request.Company &&
            ((device as WindowSensor)!).Functionalities!.SequenceEqual(request.Functionalities!)
        )));
        
        ObjectResult? result = _deviceController.PostWindowSensors(request) as CreatedAtActionResult;
        
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
    public void TestPostSecurityCamerasCreatedStatusCode()
    {
        SecurityCameraRequest request = DefaultSecurityCameraRequest();
        _mockIDeviceService.Setup(service => service.CreateDevice(It.Is<Device>(device => 
            device.Company != null &&
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company.Id == request.Company &&
            ((device as SecurityCamera)!).LocationType == request.LocationType &&
            ((device as SecurityCamera)!).Functionalities != null &&
            ((device as SecurityCamera)!).Functionalities!.SequenceEqual(request.Functionalities)
        )));
        
        ObjectResult? result = _deviceController.PostSecurityCameras(request) as CreatedAtActionResult;
        
        _mockIDeviceService.Verify(service => service.CreateDevice(It.Is<Device>(device => 
            device.Company != null &&
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company.Id == request.Company &&
            ((device as SecurityCamera)!).LocationType == request.LocationType &&
            ((device as SecurityCamera)!).Functionalities != null &&
            ((device as SecurityCamera)!).Functionalities!.SequenceEqual(request.Functionalities)
        )), Times.Once);
        Assert.AreEqual(CreatedStatusCode, result!.StatusCode);
    }
    
    private void SetupDefaultObjects()
    {
        List<string> photos =
        [
            DevicePhotoUrl
        ];
        
        _defaultCompany = new Company { Name = CompanyName };

        _defaultCamera = new SecurityCamera()
        {
            Name = CameraName, 
            Model = DeviceModel, 
            PhotoURLs = photos, 
            Company = _defaultCompany, 
            Kind = SecurityCameraType
        };
        _defaultWindowSensor = new WindowSensor()
        {
            Name = WindowSensorName, 
            Model = DeviceModel, 
            PhotoURLs = photos, 
            Company = _defaultCompany, 
            Kind = WindowSensorType
        };
    }

    private void SetupDeviceController()
    {
        _mockIDeviceService = new Mock<IDeviceService>();
        _mockISessionService = new Mock<ISessionService>();
        _mockICompanyService = new Mock<ICompanyService>();
        _deviceController = new DeviceController(_mockIDeviceService.Object, _mockICompanyService.Object);
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
            Company = _defaultCompany.Id,
            LocationType = LocationType.Indoor,
            Functionalities = [SecurityCameraFunctionality.MotionDetection],
        };
    }
    
    private PageDataRequest DefaultPageDataRequest()
    {
        PageDataRequest request = new PageDataRequest();
        
        request.Page = 1;
        request.PageSize = 10;
        
        return request;
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
}