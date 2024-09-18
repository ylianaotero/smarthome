using BusinessLogic.Services;
using Domain;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class DeviceServiceTest
{
    private Mock<IRepository<Device>> _mockDeviceRepository;
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
        CreateMockDeviceRepository();
        SetupDefaultObjects();
    }

    [TestMethod]
    public void TestGetAllDevices()
    {
        List<Device> devices = new List<Device>();
        devices.Add(_defaultCamera);
        devices.Add(_defaultWindowSensor);
        _mockDeviceRepository.Setup(x => x.GetAll()).Returns(devices);
        DeviceService deviceService = new DeviceService(_mockDeviceRepository.Object);
        
        List<Device> retrievedDevices =  deviceService.GetAllDevices();
        
        Assert.AreEqual(devices, retrievedDevices);
    }
    
    private void SetupDefaultObjects()
    {
        List<string> photos = new List<string>()
        {
            DevicePhotoUrl,
        };
        
        _defaultCompany = new Company { Name = CompanyName };

        _defaultCamera = new SecurityCamera()
            { Name = CameraName, Model = DeviceModel, PhotoURLs = photos, Company = _defaultCompany };
        _defaultWindowSensor = new WindowSensor()
            { Name = WindowSensorName, Model = DeviceModel, PhotoURLs = photos, Company = _defaultCompany };
    }

    private void CreateMockDeviceRepository()
    {
        _mockDeviceRepository = new Mock<IRepository<Device>>();
    }
}