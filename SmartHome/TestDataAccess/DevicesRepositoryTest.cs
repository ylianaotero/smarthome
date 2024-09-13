using DataAccess;
using Domain;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using IDomain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDataAccess;

[TestClass]
public class DevicesRepositoryTest
{
    private SqliteConnection _connection;
    private SmartHomeContext _context;
    private DevicesRepository _devicesRepository;
    private SecurityCamera _defaultCamera;
    private WindowSensor _defaultWindowSensor;
    private Company _defaultCompany;
    
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string DeviceDescription = "This is a device";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const long DeviceModel = 1345354616346;
    private const bool DeviceIsConnected = true;
    
    private const string CompanyName = "IoT Devices & Co.";
    private const string CompanyRUT = "123456789";
    private const string CompanyLogoURL = "https://example.com/logo.jpg";
    
    [TestInitialize]
    public void TestInitialize()
    {
        SetupRepository();
        SetupDefaultObjects();
    }
    
    [TestCleanup]
    public void TestCleanup()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void TestGetDevicesByFilter()
    {
        long otherModel = 1345354616347;
        List<Device> devices = new List<Device>
        {
            _defaultCamera,
            _defaultWindowSensor,
            new SecurityCamera
            {
                Name = CameraName,
                Model = otherModel,
                Description = DeviceDescription,
                PhotoURLs = new List<string> { DevicePhotoUrl },
                Company = _defaultCompany,
                IsConnected = DeviceIsConnected,
            },
        };
        LoadContext(devices);

        Func<Device,bool> filter = d => d.Model == DeviceModel;

        List<Device> retrievedDevices = _devicesRepository.GetDevicesByFilter(filter);
        
        Assert.IsTrue(retrievedDevices.TrueForAll(retrievedDevice => retrievedDevice.Model == DeviceModel));
    }

    [TestMethod]
    public void TestGetAllDevices()
    {
        List<Device> devices = new List<Device>
        {
            _defaultCamera,
            _defaultWindowSensor,
        };
        LoadContext(devices);
        
        List<Device> retrievedDevices = _devicesRepository.GetAllDevices();
        
        Assert.AreEqual(devices.Count(), retrievedDevices.Count());
    }

    [TestMethod]
    public void TestGetDeviceById()
    {
        List<Device> devices = new List<Device>
        {
            _defaultCamera,
            _defaultWindowSensor,
        };
        LoadContext(devices);
        
        Device? retrievedDevice = _devicesRepository.GetDeviceById(_defaultCamera.Id);
        
        Assert.AreEqual(retrievedDevice.Id, _defaultCamera.Id);
    }
    
    [TestMethod]
    public void TestAddDevice()
    {
        _devicesRepository.AddDevice(_defaultCamera);
        
        List<Device> retrievedDevices = _devicesRepository.GetAllDevices();
        
        CollectionAssert.Contains(retrievedDevices, _defaultCamera);
    }

    [TestMethod]
    public void TestDeleteDevice()
    {
        List<Device> devices = new List<Device>
        {
            _defaultCamera,
            _defaultWindowSensor,
        };
        LoadContext(devices);
        
        _devicesRepository.DeleteDevice(_defaultCamera);
        
        List<Device> retrievedDevices = _devicesRepository.GetAllDevices();
        
        CollectionAssert.DoesNotContain(retrievedDevices, _defaultCamera);
    }
    
    
    private void SetupRepository()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var contextOptions = new DbContextOptionsBuilder<SmartHomeContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new SmartHomeContext(contextOptions);
        _context.Database.EnsureCreated();

        _devicesRepository = new DevicesRepository(_context);
    }

    private void SetupDefaultObjects()
    {
        _defaultCompany = new Company
        {
            Name = CompanyName,
            RUT = CompanyRUT,
            LogoURL = CompanyLogoURL
        };
        
        _defaultCamera = new SecurityCamera
        {
            Name = CameraName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Company = _defaultCompany,
            IsConnected = DeviceIsConnected,
        };
        
        _defaultWindowSensor = new WindowSensor
        {
            Name = WindowSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Company = _defaultCompany,
            IsConnected = DeviceIsConnected,
        };
    }
    
    private void LoadContext(List<Device> devices)
    {
        _context.Devices.AddRange(devices);
        _context.SaveChanges();
    }
}