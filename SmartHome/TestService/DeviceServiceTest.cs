using BusinessLogic.Services;
using Domain;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class DeviceServiceTest
{
    private Mock<IRepository<Device>> _mockDeviceRepository;
    private DeviceService _deviceService;
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
        CreateMockAndService();
        SetupDefaultObjects();
    }

    [TestMethod]
    public void TestGetAllDevices()
    {
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        devices.Add(_defaultWindowSensor);
        _mockDeviceRepository.Setup(x => x.GetAll()).Returns(devices);
        
        List<Device> retrievedDevices =  _deviceService.GetAllDevices();
        
        Assert.AreEqual(devices, retrievedDevices);
    }
    
    [TestMethod]
    public void TestGetDevicesWithFilter()
    {
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        Func<Device, bool> filter = d => d.Model == DeviceModel && d.Name == CameraName && d.Kind == "SecurityCamera" &&
                                         d.PhotoURLs.First() == DevicePhotoUrl && d.Company.Name == CompanyName;
        
        _mockDeviceRepository.Setup(x => x.GetByFilter(filter)).Returns(devices);
        
        List<Device> retrievedDevices =  _deviceService.GetDevicesByFilter(filter);
        
        Assert.AreEqual(devices, retrievedDevices);
    }
    
    [TestMethod]
    public void TestGetDeviceTypes()
    {
        List<string> deviceTypes = new List<string>()
        {
            "SecurityCamera",
            "WindowSensor"
        };
        
        List<string> retrievedDeviceTypes = _deviceService.GetDeviceTypes();
        
        CollectionAssert.AreEqual(deviceTypes, retrievedDeviceTypes);
    }
    
    [TestMethod]
    public void TestCreateDevice()
    {
        _mockDeviceRepository.Setup(x => x.Add(_defaultWindowSensor));
        
        _deviceService.CreateDevice(_defaultWindowSensor);
        _mockDeviceRepository.Verify(x => x.Add(_defaultWindowSensor), Times.Once);
    }

    [TestMethod]
    public void TestGetDeviceById()
    {
        _mockDeviceRepository.Setup(x => x.GetById(1)).Returns(_defaultCamera);
        
        Device retrievedDevice = _deviceService.GetDeviceById(1);
        Assert.AreEqual(_defaultCamera, retrievedDevice);
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

    private void CreateMockAndService()
    {
        _mockDeviceRepository = new Mock<IRepository<Device>>();
        _deviceService = new DeviceService(_mockDeviceRepository.Object);
    }
}