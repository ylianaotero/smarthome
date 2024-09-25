using BusinessLogic;
using CustomExceptions;
using Domain;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class HomeServiceTest
{
    private Mock<IRepository<Home>> _mockHomeRepository;
    private Home _home;
    
    private Home _defaultHome;
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    private const long homeOwnerId = 000;
    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateMockHomeRepository();
        SetupDefaultObjects();
    }

    private void SetupDefaultObjects()
    {
        _defaultHome = new Home(homeOwnerId,Street, DoorNumber, Latitude, Longitude);
    }

    private void CreateMockHomeRepository()
    {
        _mockHomeRepository = new Mock<IRepository<Home>>();
    }
    
    [TestMethod]
    public void TestGetAllHomes()
    {
        List<Home> homes = new List<Home>();
        Home newHome = new Home(homeOwnerId,Street, DoorNumber, Latitude, Longitude);
        homes.Add(newHome);
        _mockHomeRepository.Setup(m => m.GetAll()).Returns(homes);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        List<Home> retrievedHomes = homeService.GetAllHomes();
        Assert.AreEqual(homes, retrievedHomes); 
    }
    
    [TestMethod]
    public void TestCreateHome()
    {
        Home newHome = new Home(homeOwnerId,Street, DoorNumber, Latitude, Longitude);
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
        _home = new Home(homeOwnerId,Street, DoorNumber, Latitude, Longitude);
        foreach (Member member in members)
        {
            _home.AddMember(member);
        }
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_home);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        List<Member> retrievedMembers = homeService.GetMembersFromHome(1);
        CollectionAssert.AreEqual(members, retrievedMembers); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotGetMemberByIdBecauseHomeDoesNotExist()
    {
        int searchedHomeId = 999;
    
        _mockHomeRepository.Setup(m => m.GetById(searchedHomeId)).Returns((Home)null);

        HomeService homeService = new HomeService(_mockHomeRepository.Object);

        homeService.GetMembersFromHome(searchedHomeId);
    }

    
    [TestMethod]
    public void TestGetDevicesFromHome()
    {
        List<Device> devices = new List<Device>
        {
            new SecurityCamera { Id = 1, Name = "Cámara de seguridad", Model = 123, Description = "Cámara para exteriores", IsConnected = true },
            new WindowSensor { Id = 2, Name = "Sensor de ventana", Model = 456, Description = "Sensor para ventanas", IsConnected = false }
        };
    
        _home = new Home(homeOwnerId,Street, DoorNumber, Latitude, Longitude);
        
        foreach (Device device in devices)
        {
            _home.AddDevice(device);
        }
    
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_home);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        List<Device> retrievedDevices = homeService.GetDevicesFromHome(1);
        CollectionAssert.AreEqual(devices, retrievedDevices);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotGetDeviceFromHomeBecauseItIsNotFound()
    {
        int searchedId = 999;
        _mockHomeRepository.Setup(m => m.GetById(searchedId)).Returns((Home)null);
    
        HomeService homeService = new HomeService(_mockHomeRepository.Object);

        homeService.GetDevicesFromHome(searchedId);
    }
    
    [TestMethod]
    public void TestAddMemberToHome()
    {
        Member member = new Member { Email = "member3@example.com", Permission = true };
        _home = new Home(homeOwnerId,Street, DoorNumber, Latitude, Longitude);
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_home);

        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        
        homeService.AddMemberToHome(1, member);
        
        Assert.IsTrue(_home.Members.Contains(member));
        _mockHomeRepository.Verify(m => m.Update(_home), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestCannotAddMemberToHomeBecauseItIsAlreadyExist()
    {
        int homeId = 1;
        Member existingMember = new Member { Email = "existingmember@example.com", Permission = true };
        Member newMember = new Member { Email = "existingmember@example.com", Permission = false };
    
        _home = new Home(homeOwnerId,Street, DoorNumber, Latitude, Longitude);
        _home.AddMember(existingMember);
    
        _mockHomeRepository.Setup(m => m.GetById(homeId)).Returns(_home);

        HomeService homeService = new HomeService(_mockHomeRepository.Object);

        homeService.AddMemberToHome(homeId, newMember);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotAddMemberToHomeBecauseItIsNotFound()
    {
        int nonExistentHomeId = 999;
        Member newMember = new Member { Email = "existingmember@example.com", Permission = true };
    
        _mockHomeRepository.Setup(m => m.GetById(nonExistentHomeId)).Returns((Home)null);

        HomeService homeService = new HomeService(_mockHomeRepository.Object);

        homeService.AddMemberToHome(nonExistentHomeId, newMember);
    }

    [TestMethod]
    public void TestGetHomesByFilter()
    {
        Func<Home, bool> filter = home => home.Street == Street;
        List<Home> homes = new List<Home>
        {
            new Home(homeOwnerId,Street, DoorNumber, Latitude, Longitude)
        };
        _mockHomeRepository.Setup(m => m.GetByFilter(filter)).Returns(homes);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        
        List<Home> retrievedHomes = homeService.GetHomesByFilter(filter);
        
        CollectionAssert.AreEqual(homes, retrievedHomes);
    }
    
    [TestMethod]
    public void TestGetHomeById()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns(_defaultHome);
        
        Home retrivedHome = homeService.GetHomeById(1);
        Assert.AreEqual(_defaultHome, retrivedHome);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestGetHomeByIdThrowsElementNotFound()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns((Home?)null);
        
        homeService.GetHomeById(1);
    }

    
}