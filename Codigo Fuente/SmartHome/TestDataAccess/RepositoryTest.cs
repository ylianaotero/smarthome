using CustomExceptions;
using DataAccess;
using Domain.Abstract;
using Domain.Concrete;
using IDataAccess;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDataAccess;

[TestClass]
public class RepositoryTest
{
    private SqliteConnection _connection;
    private SmartHomeContext _context;
    private SqlRepository<Device> _devicesRepository;
    private SqlRepository<Company> _companiesRepository;
    private SqlRepository<User> _usersRepository;
    private SecurityCamera _defaultCamera;
    private WindowSensor _defaultWindowSensor;
    private Company _defaultCompany;
    
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string DeviceDescription = "This is a device";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const string DeviceModel = "edf124";
    
    private const string CompanyName = "IoT Devices & Co.";
    private const string CompanyRUT = "123456789";
    private const string CompanyLogoURL = "https://example.com/logo.jpg";
    
    private const string OwnerName = "John";
    private const string OwnerSurname = "Doe";
    private const string OwnerEmail = "johndoe@gmail.com";
    private const string OwnerPhoto = "https://example.com/photo.jpg";
    private const string OwnerPassword = "Abcd1234%";
    
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
        _context.Dispose();
    }
    
    [TestMethod]
    public void TestGetDevicesByFilterWithPagination()
    {
        string otherModel = "ABCDEF";
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
                Company = _defaultCompany
            },
        };
        LoadContext(devices);

        Func<Device,bool> filter = d => d.Model == DeviceModel;

        List<Device> retrievedDevices = _devicesRepository.GetByFilter(filter, PageData.Default);
        
        Assert.IsTrue(retrievedDevices.TrueForAll(retrievedDevice => retrievedDevice.Model == DeviceModel));
    }

    [TestMethod]
    public void TestGetAllDevicesWithPagination()
    {
        List<Device> devices = new List<Device>
        {
            _defaultCamera,
            _defaultWindowSensor,
        };
        LoadContext(devices);
        
        List<Device> retrievedDevices = _devicesRepository.GetAll(PageData.Default);
        
        Assert.AreEqual(devices.Count(), retrievedDevices.Count());
    }
    
    [TestMethod]
    public void TestGetDevicesByFilterWithoutPagination()
    {
        string otherModel = "ABCDEF";
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
                Company = _defaultCompany
            },
        };
        LoadContext(devices);

        Func<Device,bool> filter = d => d.Model == DeviceModel;

        List<Device> retrievedDevices = _devicesRepository.GetByFilter(filter, null);
        
        Assert.IsTrue(retrievedDevices.TrueForAll(retrievedDevice => retrievedDevice.Model == DeviceModel));
    }

    [TestMethod]
    public void TestGetAllDevicesWithoutPagination()
    {
        List<Device> devices = new List<Device>
        {
            _defaultCamera,
            _defaultWindowSensor,
        };
        LoadContext(devices);
        
        List<Device> retrievedDevices = _devicesRepository.GetAll(null);
        
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
        
        Device? retrievedDevice = _devicesRepository.GetById(_defaultCamera.Id);
        
        Assert.AreEqual(retrievedDevice.Id, _defaultCamera.Id);
    }
    
    [TestMethod]
    public void TestAddDevice()
    {
        _usersRepository.Add(_defaultCompany.Owner);
        _companiesRepository.Add(_defaultCompany);
        _devicesRepository.Add(_defaultCamera);
        
        List<Device> retrievedDevices = _devicesRepository.GetAll(PageData.Default);
        
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
        
        _devicesRepository.Delete(_defaultCamera);
        
        List<Device> retrievedDevices = _devicesRepository.GetAll(PageData.Default);
        
        CollectionAssert.DoesNotContain(retrievedDevices, _defaultCamera);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestDeleteNonexistentDevice()
    {
        List<Device> devices = new List<Device>
        {
            _defaultCamera
        };
        LoadContext(devices);
        
        _devicesRepository.Delete(_defaultWindowSensor);
    }
    
    [TestMethod]
    public void TestGetAllHomeOwners()
    {
        List<HomeOwner> homeOwners = new List<HomeOwner>
        {
            new HomeOwner()
        };
        
        _context.HomeOwners.AddRange(homeOwners);
        _context.SaveChanges();
        
        SqlRepository<HomeOwner> _homeOwnersRepository = new SqlRepository<HomeOwner>(_context);
        
        List<HomeOwner> retrievedHomeOwners = _homeOwnersRepository.GetAll(PageData.Default);
        
        Assert.AreEqual(homeOwners.Count(), retrievedHomeOwners.Count());
    }
    
    [TestMethod]
    public void TestGetAllAdministrators()
    {
        List<Administrator> administrators = new List<Administrator>
        {
            new Administrator()
        };
        
        _context.Administrators.AddRange(administrators);
        _context.SaveChanges();
        
        SqlRepository<Administrator> _administratorsRepository = new SqlRepository<Administrator>(_context);
        
        List<Administrator> retrievedAdministrators = _administratorsRepository.GetAll(PageData.Default);
        
        Assert.AreEqual(administrators.Count(), retrievedAdministrators.Count());
    }
    
    [TestMethod]
    public void TestGetAllCompanyOwners()
    {
        List<CompanyOwner> companyOwners = new List<CompanyOwner>
        {
            new CompanyOwner() {Company = _defaultCompany}
        };
        
        _context.CompanyOwners.AddRange(companyOwners);
        _context.SaveChanges();
        
        SqlRepository<CompanyOwner> _companyOwnersRepository = new SqlRepository<CompanyOwner>(_context);
        
        List<CompanyOwner> retrievedHomeOwners = _companyOwnersRepository.GetAll(PageData.Default);
        
        Assert.AreEqual(companyOwners.Count(), retrievedHomeOwners.Count());
    }

    [TestMethod]
    public void TestUpdateDevices()
    {
        List<Device> devices = new List<Device>
        {
            _defaultCamera,
            _defaultWindowSensor,
        };
        LoadContext(devices);
        
        _defaultCamera.Name = "New Name";
        _devicesRepository.Update(_defaultCamera);
        
        Device? retrievedDevice = _devicesRepository.GetById(_defaultCamera.Id);
        
        Assert.AreEqual(retrievedDevice.Name, "New Name");
    }
    
    
    [TestMethod]
    public void TestPageData()
    {
        List<Device> devices = new List<Device>
        {
            _defaultCamera,
            _defaultWindowSensor,
        };
        LoadContext(devices);
        
        PageData pageData = new PageData()
        {
            PageNumber = 1,
            PageSize = 1
        };
        
        List<Device> retrievedDevices = _devicesRepository.GetAll(pageData);
        
        Assert.AreEqual(retrievedDevices.Count(), 1);
    }
    

    private void SetupRepository()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var contextOptions = new DbContextOptionsBuilder<SmartHomeContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new SmartHomeContext(contextOptions, true);
        _context.Database.EnsureCreated();

        _devicesRepository = new SqlRepository<Device>(_context);
        _companiesRepository = new SqlRepository<Company>(_context);
        _usersRepository = new SqlRepository<User>(_context);
    }
    
    private void SetupDefaultObjects()
    {
        SetupDefaultCompany();
        SetupDefaultDevices();
    }

    private void SetupDefaultCompany()
    {
        _defaultCompany = new Company
        {
            Name = CompanyName,
            RUT = CompanyRUT,
            LogoURL = CompanyLogoURL,
            ValidationMethod = "ValidatorNumber",
            Owner = new User
            {
                Name = OwnerName,
                Surname = OwnerSurname,
                Email = OwnerEmail,
                Photo = OwnerPhoto,
                Password = OwnerPassword
            }
        };
    }

    private void SetupDefaultDevices()
    {
        _defaultCamera = new SecurityCamera
        {
            Name = CameraName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Company = _defaultCompany
        };
        
        _defaultWindowSensor = new WindowSensor
        {
            Name = WindowSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Company = _defaultCompany
        };
    }
    
    private void LoadContext(List<Device> devices)
    {
        _context.Users.Add(_defaultCompany.Owner);
        _context.Companies.Add(_defaultCompany);
        _context.Devices.AddRange(devices);
        _context.SaveChanges();
    }
}