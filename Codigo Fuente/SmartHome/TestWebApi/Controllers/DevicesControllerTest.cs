using CustomExceptions;
using IBusinessLogic;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Enum;
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
    private Company _defaultCompany;
    private SecurityCamera _defaultCamera;
    private WindowSensor _defaultWindowSensor;
    private MotionSensor _defaultMotionSensor;
    private DeviceController _deviceController;
    private Mock<IDeviceService> _mockIDeviceService;
    private Mock<ICompanyService> _mockICompanyService;
    private Mock<ISessionService> _mockISessionService;
    
    private const long DeviceModel = 1345354616346;
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string MotionSensorName = "My Motion Sensor";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const string CompanyName = "IoT DeviceUnits & Co.";
    private const string SecurityCameraType = "SecurityCamera";
    private const string WindowSensorType = "WindowSensor";
    private const string MotionSensorType = "MotionSensor";
    private const string DeviceNotFoundExceptionMessage = "Device not found";
    private const string CompanyNotFoundExceptionMessage = "Company not found";
    
    private List<Device> _devices;
    private List<string> _deviceTypes;
    private List<string> _photos;
    private LocationType _locationType;
    private List<SecurityCameraFunctionality> _securityCameraFunctionalities;
    private List<WindowSensorFunctionality> _windowSensorFunctionalities;
    private List<MotionSensorFunctionality> _motionSensorFunctionalities;
    
    private const int OkStatusCode = 200;
    private const int CreatedStatusCode = 201;
    private const int NotFoundStatusCode = 404;
    
    [TestInitialize]
    public void TestInitialize()
    {
        SetupDeviceController();
        SetupDefaultObjects();
    }
    
    [TestMethod]
    public void TestGetAllDevicesOkStatusCode()
    {
        _mockIDeviceService
            .Setup(service => service
                .GetDevicesByFilter(It.IsAny<Func<Device, bool>>(), It.IsAny<PageData>()))
            .Returns(_devices);
        
        GetDeviceRequest request = new GetDeviceRequest();
        
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

        GetDeviceRequest request = new GetDeviceRequest();
        
        ObjectResult? result = _deviceController.GetDevices(request, DefaultPageDataRequest()) as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }

    [TestMethod]
    public void TestGetAllDevicesOkResponse()
    {
        _mockIDeviceService
            .Setup(service => service
                .GetDevicesByFilter(It.IsAny<Func<Device, bool>>(), It.IsAny<PageData>()))
            .Returns(_devices);
        GetDevicesResponse expectedResponse = DefaultDevicesResponse();

        GetDeviceRequest request = new GetDeviceRequest();
        
        ObjectResult? result = _deviceController.GetDevices(request, DefaultPageDataRequest()) as OkObjectResult;
        GetDevicesResponse response = (result!.Value as GetDevicesResponse)!;
        
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
        GetDeviceResponse expectedResponse = DefaultSecurityCameraResponse();
        
        ObjectResult? result = _deviceController.GetDeviceById(1) as OkObjectResult;
        GetDeviceResponse response = (result!.Value as GetDeviceResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDeviceByIdNotFoundStatusCode()
    {
        _mockIDeviceService.Setup(service => service.GetDeviceById(1))
            .Throws(new ElementNotFound(DeviceNotFoundExceptionMessage));
        _mockISessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
     
        IActionResult result = _deviceController.GetDeviceById(1);

        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }
    
    [TestMethod]
    public void TestGetDeviceTypesOkStatusCode()
    {
        _mockIDeviceService.Setup(service => service.GetDeviceTypes()).Returns(_deviceTypes);
        
        ObjectResult? result = _deviceController.GetDeviceTypes() as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetDeviceTypesOkResponse()
    {
        _mockIDeviceService.Setup(service => service.GetDeviceTypes()).Returns(_deviceTypes);
        GetDeviceTypesResponse expectedResponse = new GetDeviceTypesResponse()
        {
            DeviceTypes = _deviceTypes,
        };
        
        ObjectResult? result = _deviceController.GetDeviceTypes() as OkObjectResult;
        GetDeviceTypesResponse response = (result!.Value as GetDeviceTypesResponse)!;
        
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
        GetDeviceRequest request = new GetDeviceRequest()
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
        GetDeviceRequest request = new GetDeviceRequest()
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
        GetDevicesResponse? expectedResponse = new GetDevicesResponse(devices);
        
        ObjectResult? result = _deviceController.GetDevices(request, DefaultPageDataRequest()) as OkObjectResult;
        GetDevicesResponse? response = result!.Value as GetDevicesResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestPostWindowSensorsCreatedStatusCode()
    {
        PostWindowSensorRequest request = DefaultWindowSensorRequest();
        _mockIDeviceService.Setup(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company.Id == request.Company 
        )));
        
        _mockICompanyService
            .Setup(service => service.AddCompanyToDevice(It.IsAny<long>(), It.IsAny<Device>()))
            .Returns(_defaultWindowSensor);
        
        ObjectResult? result = _deviceController.PostWindowSensors(request) as CreatedAtActionResult;
        
        _mockIDeviceService.Verify(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company.Id == request.Company
        )), Times.Once);
        Assert.AreEqual(CreatedStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostSecurityCamerasCreatedStatusCode()
    {
        PostSecurityCameraRequest request = DefaultSecurityCameraRequest();
        _mockIDeviceService.Setup(service => service.CreateDevice(It.Is<Device>(device => 
            device.Company != null &&
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company.Id == request.Company 
        )));
        
        _mockICompanyService
            .Setup(service => service.AddCompanyToDevice(It.IsAny<long>(), It.IsAny<Device>()))
            .Returns(_defaultCamera);
        
        ObjectResult? result = _deviceController.PostSecurityCameras(request) as CreatedAtActionResult;
        
        _mockIDeviceService.Verify(service => service.CreateDevice(It.Is<Device>(device => 
            device.Company != null &&
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company.Id == request.Company
        )), Times.Once);
        Assert.AreEqual(CreatedStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostWindowSensorsNotFoundStatusCode()
    {
        PostSecurityCameraRequest request = DefaultSecurityCameraRequest();
        _mockICompanyService
            .Setup(service => service.AddCompanyToDevice(It.IsAny<long>(), It.IsAny<Device>()))
            .Throws(new ElementNotFound(CompanyNotFoundExceptionMessage));
        
        NotFoundObjectResult? result = _deviceController.PostSecurityCameras(request) as NotFoundObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostSecurityCamerasNotFoundStatusCode()
    {
        PostWindowSensorRequest request = DefaultWindowSensorRequest();
        _mockICompanyService
            .Setup(service => service.AddCompanyToDevice(It.IsAny<long>(), It.IsAny<Device>()))
            .Throws(new ElementNotFound(CompanyNotFoundExceptionMessage));
        
        NotFoundObjectResult? result = _deviceController.PostWindowSensors(request) as NotFoundObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostMotionSensorsCreatedStatusCode()
    {
        PostMotionSensorRequest request = DefaultMotionSensorRequest();
        _mockIDeviceService.Setup(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company.Id == request.Company 
        )));
        
        _mockICompanyService
            .Setup(service => service.AddCompanyToDevice(It.IsAny<long>(), It.IsAny<Device>()))
            .Returns(_defaultMotionSensor);
        
        ObjectResult? result = _deviceController.PostMotionSensors(request) as CreatedAtActionResult;
        
        _mockIDeviceService.Verify(service => service.CreateDevice(It.Is<Device>(device => 
            device.Name == request.Name &&
            device.Model == request.Model &&
            device.PhotoURLs == request.PhotoUrls &&
            device.Description == request.Description &&
            device.Company.Id == request.Company
        )), Times.Once);
        Assert.AreEqual(CreatedStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostMotionSensorsNotFoundStatusCode()
    {
        PostMotionSensorRequest request = DefaultMotionSensorRequest();
        _mockICompanyService
            .Setup(service => service.AddCompanyToDevice(It.IsAny<long>(), It.IsAny<Device>()))
            .Throws(new ElementNotFound(CompanyNotFoundExceptionMessage));
        
        NotFoundObjectResult? result = _deviceController.PostMotionSensors(request) as NotFoundObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result!.StatusCode);
    }
    
    private void SetupDefaultObjects()
    {
        SetupDefaultAuxObjects();
        SetupDefaultDeviceObjects();
        
        _devices =
        [
            _defaultCamera,
            _defaultWindowSensor,
            _defaultMotionSensor
        ];
        
        _deviceTypes =
        [
            SecurityCameraType,
            WindowSensorType,
            MotionSensorType
        ];
    }

    private void SetupDefaultAuxObjects()
    {
        _photos = new List<string>() {DevicePhotoUrl};
        _defaultCompany = new Company
        {
            Name = CompanyName,
            Id = 1
        };
        _motionSensorFunctionalities = new List<MotionSensorFunctionality>() { MotionSensorFunctionality.MotionDetection };
        _windowSensorFunctionalities = new List<WindowSensorFunctionality>() { WindowSensorFunctionality.OpenClosed };
        _securityCameraFunctionalities = new List<SecurityCameraFunctionality>() { SecurityCameraFunctionality.HumanDetection };
        _locationType = LocationType.Indoor;
    }

    private void SetupDefaultDeviceObjects()
    {
        _defaultCamera = new SecurityCamera()
        {
            Name = CameraName, 
            Model = DeviceModel, 
            PhotoURLs = _photos, 
            Company = _defaultCompany, 
            Kind = SecurityCameraType,
            Functionalities = _securityCameraFunctionalities,
            LocationType = _locationType
        };
        _defaultWindowSensor = new WindowSensor()
        {
            Name = WindowSensorName, 
            Model = DeviceModel, 
            PhotoURLs = _photos, 
            Company = _defaultCompany, 
            Kind = WindowSensorType,
            Functionalities = _windowSensorFunctionalities
        };
        _defaultMotionSensor = new MotionSensor()
        {
            Name = MotionSensorName,
            Model = DeviceModel,
            PhotoURLs = _photos,
            Company = _defaultCompany,
            Functionalities = _motionSensorFunctionalities
        };
    }

    private void SetupDeviceController()
    {
        _mockIDeviceService = new Mock<IDeviceService>();
        _mockISessionService = new Mock<ISessionService>();
        _mockICompanyService = new Mock<ICompanyService>();
        _deviceController = new DeviceController(_mockIDeviceService.Object, _mockICompanyService.Object);
    }
    
    private PostWindowSensorRequest DefaultWindowSensorRequest()
    {
        return new PostWindowSensorRequest()
        {
            Name = _defaultWindowSensor.Name,
            Model = _defaultWindowSensor.Model,
            PhotoUrls = _defaultWindowSensor.PhotoURLs,
            Description = _defaultWindowSensor.Description,
            Functionalities = _windowSensorFunctionalities.Select(func => func.ToString()).ToList(),
            Company = _defaultCompany.Id,
        };
    }

    private PostSecurityCameraRequest DefaultSecurityCameraRequest()
    {
        return new PostSecurityCameraRequest()
        {
            Name = _defaultCamera.Name,
            Model = _defaultCamera.Model,
            PhotoUrls = _defaultCamera.PhotoURLs,
            Description = _defaultCamera.Description,
            Company = _defaultCompany.Id,
            LocationType = _defaultCamera.LocationType.ToString(),
            Functionalities = _securityCameraFunctionalities.Select(func => func.ToString()).ToList(),
        };
    }

    private PostMotionSensorRequest DefaultMotionSensorRequest()
    {
        return new PostMotionSensorRequest()
        {
            Name = _defaultMotionSensor.Name,
            Model = _defaultMotionSensor.Model,
            PhotoUrls = _defaultMotionSensor.PhotoURLs,
            Description = _defaultMotionSensor.Description,
            Functionalities = _motionSensorFunctionalities.Select(func => func.ToString()).ToList(),
            Company = _defaultCompany.Id,
        };
    }
    
    private static PageDataRequest DefaultPageDataRequest()
    {
        PageDataRequest request = new PageDataRequest();
        
        request.Page = 1;
        request.PageSize = 10;
        
        return request;
    }
    
    private GetDevicesResponse DefaultDevicesResponse()
    {
        List<Device> devices =
        [
            _defaultCamera,
            _defaultWindowSensor,
            _defaultMotionSensor
        ];

        return new GetDevicesResponse(devices);
    }
    
    private GetDeviceResponse DefaultSecurityCameraResponse()
    {
        return new GetDeviceResponse(_defaultCamera);
    }
}