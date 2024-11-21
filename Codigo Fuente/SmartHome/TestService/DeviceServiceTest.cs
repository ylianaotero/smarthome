using BusinessLogic;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class DeviceServiceTest
{
    private List<Device> _devices;
    private Company _defaultCompany;
    private DeviceService _deviceService;
    private SecurityCamera _defaultCamera;
    private WindowSensor _defaultWindowSensor;
    private Mock<IRepository<Device>> _mockDeviceRepository;
    
    private const string DeviceModel = "edf124";
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const string SecurityCameraType = "SecurityCamera";
    private const string WindowSensorType = "WindowSensor";
    private const string MotionSensorType = "MotionSensor";
    private const string SmartLampType = "SmartLamp";
    private const string CompanyName = "IoT Devices & Co.";
    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateMockAndService();
        SetupDefaultObjects();
    }
    
    [TestMethod]
    public void TestGetDevicesWithFilter()
    {
        _devices.Add(_defaultCamera);
        Func<Device, bool> filter = d => d.Model == DeviceModel && d.Name == CameraName 
                                                                && d.Kind == SecurityCameraType 
                                                                && d.PhotoURLs.First() == DevicePhotoUrl 
                                                                && d.Company.Name == CompanyName;
        
        _mockDeviceRepository
            .Setup(x => x.GetByFilter(filter, null))
            .Returns(_devices);
        
        List<Device> retrievedDevices =  _deviceService.GetDevicesByFilter(filter, null);
        
        Assert.AreEqual(_devices, retrievedDevices);
    }
    
    [TestMethod]
    public void TestGetDeviceTypes()
    {
        List<string> deviceTypes = new List<string>()
        {
            MotionSensorType,
            SecurityCameraType,
            SmartLampType,
            WindowSensorType,
        };
        
        List<string> retrievedDeviceTypes = _deviceService.GetDeviceTypes();
        
        CollectionAssert.AreEqual(deviceTypes, retrievedDeviceTypes);
    }

    [TestMethod]
    public void TestGetDeviceById()
    {
        _mockDeviceRepository.Setup(x => x.GetById(1)).Returns(_defaultCamera);
        
        Device retrievedDevice = _deviceService.GetDeviceById(1);
        Assert.AreEqual(_defaultCamera, retrievedDevice);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestGetDeviceByIdThrowsException()
    {
        _mockDeviceRepository.Setup(x => x.GetById(1)).Returns((Device?)null);

        _deviceService.GetDeviceById(1);
    }
    
    [TestMethod]
    public void TestCreateDevice()
    {
        _deviceService.CreateDevice(_defaultCamera);
        
        _mockDeviceRepository.Verify(x => x.Add(_defaultCamera), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestCreateDeviceFailsBecauseOfValidationMethod()
    {
        Company company = new Company
        {
            Name = CompanyName,
            Id = 1,
            ValidationMethod = "VALIDATORNUMBER"
        };
        
        SecurityCamera camera = new SecurityCamera
        {
            Name = CameraName,
            Model = "abcdef",
            PhotoURLs = new List<string>() { DevicePhotoUrl },
            Company = company,
            Kind = SecurityCameraType
        };
        
        _deviceService.CreateDevice(camera);
    }
    
    [TestMethod]
    public void TestCreateDeviceWithValidationMethod()
    {
        Company company = new Company
        {
            Name = CompanyName,
            Id = 1,
            ValidationMethod = "VALIDATORLETTER"
        };
        
        SecurityCamera camera = new SecurityCamera
        {
            Name = CameraName,
            Model = "abcdef",
            PhotoURLs = new List<string>() { DevicePhotoUrl },
            Company = company,
            Kind = SecurityCameraType
        };
        
        _deviceService.CreateDevice(camera);
        
        _mockDeviceRepository.Verify(x => x.Add(_defaultCamera), Times.Once);
    }
    
    [TestMethod]
    public void TestModelValidatorLetterIsValid()
    {
        IModelValidator validator = new ValidatorLetter.ValidatorLetter();
        Modelo modelo = new Modelo();
        modelo.set_Value("abcdef");
        
        Assert.IsTrue(validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void TestModelValidatorLetterIsNotValid()
    {
        IModelValidator validator = new ValidatorLetter.ValidatorLetter();
        Modelo modelo = new Modelo();
        modelo.set_Value("123456");
        
        Assert.IsFalse(validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void TestModelValidatorNumberIsValid()
    {
        IModelValidator validator = new ValidatorNumber.ValidatorNumber();
        Modelo modelo = new Modelo();
        modelo.set_Value("abc123");
        
        Assert.IsTrue(validator.EsValido(modelo));
    }
    
    [TestMethod]
    public void TestModelValidatorNumberIsInvalid()
    {
        IModelValidator validator = new ValidatorNumber.ValidatorNumber();
        Modelo modelo = new Modelo();
        modelo.set_Value("abc1233");
        
        Assert.IsFalse(validator.EsValido(modelo));
    }
    
    private void SetupDefaultObjects()
    {
        _devices = new List<Device>();
        
        _defaultCompany = new Company
        {
            Name = CompanyName,
            ValidationMethod = "VALIDATORNUMBER"
        };
        
        List<string> photos = new List<string>()
        {
            DevicePhotoUrl,
        };

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
    

    private void CreateMockAndService()
    {
        _mockDeviceRepository = new Mock<IRepository<Device>>();
        _deviceService = new DeviceService(_mockDeviceRepository.Object);
    }
}