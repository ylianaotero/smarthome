using BusinessLogic;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class HomeServiceTest
{
    private Mock<IRepository<Home>> _mockHomeRepository;
    private Mock<IUserService> _mockUserService;
    
    private HomeService _homeService;
    private Home _defaultHome;
    private WindowSensor _windowSensor;
    private SecurityCamera _securityCamera;
    private DeviceUnit _windowSensorUnit;
    private DeviceUnit _securityCameraUnit;
    private User _user1;
    private User _user2;
    private User _defaultOwner;
    private Member _member2;
    private Member _member1; 
    
    private const string Alias = "My Home";
    private const string UpdatedAlias = "Maison";
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    private const string NewEmail = "juan.perez@example.com";
    private const string NewEmail2 = "juan.lopez@example.com";
    private const int MaxMembers = 10;
    private const int Id = 999;
    private const bool IsConnectedFalse = false;
    private const string RoomName = "Living Room";
    private const string UserNotFoundMessage = "User not found";

    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateRepositoryMocks();
        SetupDefaultObjects();
    }
    
    [TestMethod]
    public void TestCreateHome()
    {
        _mockHomeRepository.Setup(m => m.Add(_defaultHome));
        _homeService.CreateHome(_defaultHome);
        _mockHomeRepository.Verify(m => m.Add(_defaultHome), Times.Once);
    }
    
    [TestMethod]
    public void TestGetMembersByHomeId()
    { 
        List<Member> members = new List<Member>
        {
            _member1, _member2
        };
        foreach (Member member in members)
        {
            _defaultHome.AddMember(member);
        }
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_defaultHome);

        List<Member> retrievedMembers = _homeService.GetMembersFromHome(1);

        CollectionAssert.AreEqual(members, retrievedMembers); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotGetMemberByIdBecauseHomeDoesNotExist()
    {
        int searchedHomeId = Id;
    
        _mockHomeRepository.Setup(m => m.GetById(searchedHomeId)).Returns((Home)null);
        
        _homeService.GetMembersFromHome(searchedHomeId);
    }

    
    [TestMethod]
    public void TestGetDevicesFromHome()
    {
        List<DeviceUnit> devices = new List<DeviceUnit>
        {
            _windowSensorUnit,
            _securityCameraUnit
        };
    
        _defaultHome.Devices = devices;
        
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_defaultHome);

        List<DeviceUnit> retrievedDevices = _homeService.GetDevicesFromHome(1);
        CollectionAssert.AreEqual(devices, retrievedDevices);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotGetDeviceFromHomeBecauseItIsNotFound()
    {
        int searchedId = Id;
        _mockHomeRepository.Setup(m => m.GetById(searchedId)).Returns((Home)null);
        
        _homeService.GetDevicesFromHome(searchedId);
    }

    [TestMethod]
    public void TestGetHomesByFilter()
    {
        Func<Home, bool> filter = home => home.Street == Street;
        List<Home> homes = new List<Home>
        {
            _defaultHome
        };
        _mockHomeRepository
            .Setup(m => m.GetByFilter(filter, It.IsAny<PageData>()))
            .Returns(homes);
        
        List<Home> retrievedHomes = _homeService.GetHomesByFilter(filter);
        
        CollectionAssert.AreEqual(homes, retrievedHomes);
    }
    
    [TestMethod]
    public void TestGetHomeById()
    {
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns(_defaultHome);
        
        Home retrivedHome = _homeService.GetHomeById(1);
        Assert.AreEqual(_defaultHome, retrivedHome);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestGetHomeByIdThrowsElementNotFound()
    {
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns((Home?)null);
        
        _homeService.GetHomeById(1);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestTryToCreateExistingHome()
    {
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);

        _homeService.CreateHome(_defaultHome);
        _mockHomeRepository.Verify(m => m.GetById(_defaultHome.Id), Times.Once);
    }

    [TestMethod]
    public void TestAddOwnerToHome()
    {
        HomeOwner role = new HomeOwner();
        User homeOwner = new User()
        {
            Email = NewEmail,
            Id = 1,
            Roles = new List<Role>(){role},
        };
        _mockUserService.Setup(m => m.GetUserById(homeOwner.Id)).Returns(homeOwner);
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        Home homeWithOwner = _homeService.AddOwnerToHome(homeOwner.Id, _defaultHome);
        
        Assert.AreEqual(homeOwner, homeWithOwner.Owner);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddNonexistentOwnerToHome()
    {
        HomeOwner role = new HomeOwner();
        User homeOwner = new User()
        {
            Email = NewEmail,
            Id = 1,
            Roles = new List<Role>(){role},
        };
        
        _defaultHome.Owner = null;

        _mockUserService
            .Setup(m => m.GetUserById(homeOwner.Id))
            .Throws(new ElementNotFound(UserNotFoundMessage));
        
        _homeService.AddOwnerToHome(homeOwner.Id, _defaultHome);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestAddUserThatIsNotHomeOwnerToHome()
    {
        User user = new User()
        {
            Email = NewEmail,
            Id = 1,
        };
        
        
        _defaultHome.Owner = null;
        _mockUserService.Setup(m => m.GetUserById(user.Id)).Returns(user);
        
        _homeService.AddOwnerToHome(user.Id, _defaultHome);
    }

    [TestMethod]
    public void TestUpdateHomeAlias()
    {
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        _homeService.UpdateHomeAlias(_defaultHome.Id, UpdatedAlias);
        
        Assert.AreEqual(UpdatedAlias, _defaultHome.Alias);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateHomeAliasBecauseHomeNotFound()
    {
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns((Home?)null);
        
        _homeService.UpdateHomeAlias(_defaultHome.Id, UpdatedAlias);
    }
    
    [TestMethod]
    public void TestAddRoomsToHome()
    {
        Room room = new Room()
        {
            Id = Id,
            Name = RoomName
        };
        
        _defaultHome.Rooms = new List<Room>();
        
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        _homeService.AddRoomToHome(_defaultHome.Id, room);
        
        Assert.AreEqual(room, _defaultHome.Rooms[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddRoomsToNonexistentHome()
    {
        Room room = new Room()
        {
            Id = Id,
            Name = RoomName
        };
        
        _defaultHome.Rooms = new List<Room>();
        
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(null as Home);
        
        _homeService.AddRoomToHome(_defaultHome.Id, room);
    }
    
    [TestMethod]
    public void TestGetRoomsFromHome()
    {
        Room room = new Room()
        {
            Id = Id,
            Name = RoomName
        };
        
        _defaultHome.Rooms = new List<Room>(){room};
        
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        List<Room> rooms = _homeService.GetRoomsFromHome(_defaultHome.Id);
        
        CollectionAssert.AreEqual(_defaultHome.Rooms, rooms);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestGetRoomsFromNonexistentHome()
    {
        Room room = new Room()
        {
            Id = Id,
            Name = RoomName
        };
        
        _defaultHome.Rooms = new List<Room>(){room};
        
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(null as Home);
        
        _homeService.GetRoomsFromHome(_defaultHome.Id);
    }

    [TestMethod]
    public void TestUpdateHome()
    {
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        _homeService.UpdateHome(_defaultHome.Id);
        
        _mockHomeRepository.Verify(m => m.Update(_defaultHome), Times.Once);
    }

    [TestMethod]
    public void TestGetDevicesFromHomeByFilter()
    {
        List<DeviceUnit> devices = new List<DeviceUnit>
        {
            _windowSensorUnit,
            _securityCameraUnit
        };
        
        List<DeviceUnit> expected = new List<DeviceUnit>
        {
            _windowSensorUnit,
        };
    
        _defaultHome.Devices = devices;
        
        Func<DeviceUnit, bool> filter = device => device.IsConnected;
        
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_defaultHome);

        List<DeviceUnit> retrievedDevices = _homeService.GetDevicesFromHomeByFilter(1, filter);
        CollectionAssert.AreEqual(expected, retrievedDevices);
    }
    
    private void SetupDefaultObjects()
    {
        _homeService = new HomeService(_mockHomeRepository.Object, _mockUserService.Object);
        
        SetUpDefaultHome();
        SetUpDefaultDevices();
        SetUpDefaultDeviceUnits();
        SetUpDefaultMembers();
    }
    
    private void SetUpDefaultDevices()
    {
        _securityCamera = new SecurityCamera
        {
            Id = 1, 
            Name = "Cámara de seguridad", 
            Model = "123", 
            Description = "Cámara para exteriores"
        };
        
        _windowSensor = new WindowSensor
        {
            Id = 2, 
            Name = "Sensor de ventana", 
            Model = "456", 
            Description = "Sensor para ventanas"
        };
    }
    
    private void SetUpDefaultDeviceUnits()
    {
        _windowSensorUnit = new DeviceUnit
        {
            Device = _windowSensor,
            HardwareId = Guid.NewGuid(),
            IsConnected = true
        };
        
        _securityCameraUnit = new DeviceUnit
        {
            Device = _securityCamera,
            HardwareId = Guid.NewGuid(),
            IsConnected = false
        };
    }

    private void SetUpDefaultHome()
    {
        _defaultOwner = new User()
        {
            Email = NewEmail,
            Id = 1,
            Roles = new List<Role>()
            {
                new HomeOwner(),
            }
        };
        
        _defaultHome = new Home()
        {
            Id = 1,
            Owner = _defaultOwner,
            Alias = Alias,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = MaxMembers
        };
    }

    private void SetUpDefaultMembers()
    {
        _user1 = new User() {Email = NewEmail};
        _user2 = new User() {Email = NewEmail2};
        _member1 = new Member(_user1);
        _member2 = new Member(_user2);
    }

    private void CreateRepositoryMocks()
    {
        _mockHomeRepository = new Mock<IRepository<Home>>();
        _mockUserService = new Mock<IUserService>();
    }
}