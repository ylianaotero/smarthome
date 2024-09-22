using BusinessLogic.Services;
using Domain;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class HomeServiceTest
{
    private Mock<IRepository<Home>> _mockHomeRepository;
    private Home _home;
    
    private SecurityCamera _defaultCamera;
    private WindowSensor _defaultWindowSensor;
    private Company _defaultCompany;
    
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const long DeviceModel = 1345354616346;
    private const string CompanyName = "IoT Devices & Co.";
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateMockHomeRepository();
    }

    private void CreateMockHomeRepository()
    {
        _mockHomeRepository = new Mock<IRepository<Home>>();
    }
    
    [TestMethod]
    public void TestGetAllHomes()
    {
        List<Home> homes = new List<Home>();
        Home newHome = new Home(Street, DoorNumber, Latitude, Longitude);
        homes.Add(newHome);
        _mockHomeRepository.Setup(m => m.GetAll()).Returns(homes);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        List<Home> retrievedHomes = homeService.GetAllHomes();
        Assert.AreEqual(homes, retrievedHomes); 
    }
    
    [TestMethod]
    public void TestCreateHome()
    {
        Home newHome = new Home(Street, DoorNumber, Latitude, Longitude);
        _mockHomeRepository.Setup(m => m.Add(newHome));
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        homeService.CreateHome(newHome);
        _mockHomeRepository.Verify(m => m.Add(newHome), Times.Once);
    }
    
    [TestMethod]
    public void TestGetMembersByHomeId()
    { 
        List<Member> members = new List<Member>
        {
            new Member { Email = "member1@example.com", Permission = true },
            new Member { Email = "member2@example.com", Permission = false }
        };
        _home = new Home(Street, DoorNumber, Latitude, Longitude);
        foreach (Member member in members)
        {
            _home.AddMember(member);
        }
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_home);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        List<Member> retrievedMembers = homeService.GetMembersByHomeId(1);
        CollectionAssert.AreEqual(members, retrievedMembers); 
    }
    
    [TestMethod]
    public void TestGetDevicesByHomeId()
    {
        List<Device> devices = new List<Device>
        {
            new SecurityCamera { Id = 1, Name = "Cámara de seguridad", Model = 123, Description = "Cámara para exteriores", IsConnected = true },
            new WindowSensor { Id = 2, Name = "Sensor de ventana", Model = 456, Description = "Sensor para ventanas", IsConnected = false }
        };
    
        _home = new Home(Street, DoorNumber, Latitude, Longitude);
        
        foreach (Device device in devices)
        {
            _home.AddDevice(device);
        }
    
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_home);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        List<Device> retrievedDevices = homeService.GetDevicesByHomeId(1);
        CollectionAssert.AreEqual(devices, retrievedDevices);
    }
    
    [TestMethod]
    public void TestAddMemberToHome()
    {
        Member member = new Member { Email = "member3@example.com", Permission = true };
        _home = new Home(Street, DoorNumber, Latitude, Longitude);
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_home);

        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        
        homeService.AddMemberToHome(1, member);
        
        Assert.IsTrue(_home.Members.Contains(member));
        _mockHomeRepository.Verify(m => m.Update(_home), Times.Once);
    }
    
}