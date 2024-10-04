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
    private Mock<IRepository<Device>> _mockDeviceRepository;
    private Home _home;
    private WindowSensor _windowSensor;
    private SecurityCamera _securityCamera;
    private DeviceUnit _windowSensorUnit;
    private DeviceUnit _securityCameraUnit;
    
    private Home _defaultHome;
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    private const long homeOwnerId = 000;
    private const string NewEmail = "juan.perez@example.com";
    private const string NewEmail2 = "juan.lopez@example.com";
    private const int MaxMembers = 10;

    private User _user1;
    private User _user2;

    private Member _member2;
    private Member _member1; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateRepositoryMocks();
        SetupDefaultObjects();
    }
    
    [TestMethod]
    public void TestGetAllHomes()
    {
        List<Home> homes = new List<Home>();
        Home newHome = new Home()
        {
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
        homes.Add(newHome);
        _mockHomeRepository.Setup(m => m.GetAll(It.IsAny<PageData>())).Returns(homes);
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        List<Home> retrievedHomes = homeService.GetAllHomes();
        Assert.AreEqual(homes, retrievedHomes); 
    }
    
    [TestMethod]
    public void TestCreateHome()
    {
        _mockHomeRepository.Setup(m => m.Add(_defaultHome));
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        homeService.CreateHome(_defaultHome);
        _mockHomeRepository.Verify(m => m.Add(_defaultHome), Times.Once);
    }
    
    [TestMethod]
    public void TestGetMembersByHomeId()
    { 
        List<Member> members = new List<Member>
        {
            _member1, _member2
        };
        _home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = MaxMembers,
        };
        foreach (Member member in members)
        {
            _home.AddMember(member);
        }
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_home);
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);

        List<Member> retrievedMembers = homeService.GetMembersFromHome(1);

        CollectionAssert.AreEqual(members, retrievedMembers); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotGetMemberByIdBecauseHomeDoesNotExist()
    {
        int searchedHomeId = 999;
    
        _mockHomeRepository.Setup(m => m.GetById(searchedHomeId)).Returns((Home)null);

        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);

        homeService.GetMembersFromHome(searchedHomeId);
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
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        List<DeviceUnit> retrievedDevices = homeService.GetDevicesFromHome(1);
        CollectionAssert.AreEqual(devices, retrievedDevices);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotGetDeviceFromHomeBecauseItIsNotFound()
    {
        int searchedId = 999;
        _mockHomeRepository.Setup(m => m.GetById(searchedId)).Returns((Home)null);
    
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);

        homeService.GetDevicesFromHome(searchedId);
    }
    
    [TestMethod]
    public void TestAddMemberToHome()
    {
        _home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = MaxMembers,
        };
        _mockHomeRepository.Setup(m => m.GetById(_home.Id)).Returns(_home);

        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        
        homeService.AddMemberToHome(_home.Id, _member1);
        
        _mockHomeRepository.Verify(m => m.Update(_home), Times.Once);
        
        Assert.IsTrue(_home.Members.Contains(_member1));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestCannotAddMemberToHomeBecauseItIsAlreadyExist()
    {
        _home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
        };
        _home.AddMember(_member1);
    
        _mockHomeRepository.Setup(m => m.GetById(_home.Id)).Returns(_home);

        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);

        homeService.AddMemberToHome(_home.Id, _member1);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotAddMemberToHomeBecauseItIsNotFound()
    {
        int nonExistentHomeId = 999;
    
        _mockHomeRepository.Setup(m => m.GetById(nonExistentHomeId)).Returns((Home)null);

        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);

        homeService.AddMemberToHome(nonExistentHomeId, _member1);
    }

    [TestMethod]
    public void TestGetHomesByFilter()
    {
        Func<Home, bool> filter = home => home.Street == Street;
        List<Home> homes = new List<Home>
        {
            new Home()
            {
                OwnerId = homeOwnerId,
                Street = Street,
                DoorNumber = DoorNumber,
                Latitude = Latitude,
                Longitude = Longitude,
            }
        };
        _mockHomeRepository
            .Setup(m => m.GetByFilter(filter, It.IsAny<PageData>()))
            .Returns(homes);
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        
        List<Home> retrievedHomes = homeService.GetHomesByFilter(filter);
        
        CollectionAssert.AreEqual(homes, retrievedHomes);
    }
    
    [TestMethod]
    public void TestGetHomeById()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns(_defaultHome);
        
        Home retrivedHome = homeService.GetHomeById(1);
        Assert.AreEqual(_defaultHome, retrivedHome);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestGetHomeByIdThrowsElementNotFound()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns((Home?)null);
        
        homeService.GetHomeById(1);
    }

    [TestMethod]
    public void TestPutDevicesInHome()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        Home home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
        };
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns(home);
        _mockDeviceRepository.Setup(x=>x.GetById(_securityCamera.Id)).Returns(_securityCamera);
        _mockDeviceRepository.Setup(x => x.GetById(_windowSensor.Id)).Returns(_windowSensor);
        
        List<DeviceUnit> homeDevices = new List<DeviceUnit>
        {
            _windowSensorUnit,
            _securityCameraUnit
        };
        
        List<DeviceUnitDTO> homeDevicesDTO = new List<DeviceUnitDTO>
        {
            new DeviceUnitDTO
            {
                DeviceId = _windowSensor.Id,
                IsConnected = _windowSensorUnit.IsConnected
            },
            new DeviceUnitDTO
            {
                DeviceId = _securityCamera.Id,
                IsConnected = _securityCameraUnit.IsConnected
            }
        };
        
        homeService.PutDevicesInHome(1, homeDevicesDTO);
       
        CollectionAssert.AreEqual(home.Devices,homeDevices);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestPutDevicesInHomeThrowsElementNotFound()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns((Home?)null);
       
        List<DeviceUnitDTO> homeDevicesDTO = new List<DeviceUnitDTO> {};
        
        homeService.PutDevicesInHome(1, homeDevicesDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestPutDevicesInHomeThrowsElementNotFoundBecauseOfDevice()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        Home home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
        };
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns(home);
        _mockDeviceRepository.Setup(x=>x.GetById(_securityCamera.Id)).Returns((Device?)null);
        
        List<DeviceUnitDTO> homeDevicesDTO = new List<DeviceUnitDTO>
        {
            new DeviceUnitDTO
            {
                DeviceId = _securityCamera.Id,
                IsConnected = _securityCameraUnit.IsConnected
            }
        };
        
        homeService.PutDevicesInHome(1, homeDevicesDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestAddMemberToHomeFailsIfMaxMembersHasBeenReached()
    {
        _home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = 1,
        };
        _mockHomeRepository.Setup(m => m.GetById(_home.Id)).Returns(_home);

        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        
        homeService.AddMemberToHome(_home.Id, _member1);
        homeService.AddMemberToHome(_home.Id, _member2);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestTryToCreateExistingHome()
    {
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object);
        homeService.CreateHome(_defaultHome);
        _mockHomeRepository.Verify(m => m.GetById(_defaultHome.Id), Times.Once);
    }
    
    private void SetupDefaultObjects()
    {
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
            Model = 123, 
            Description = "Cámara para exteriores"
        };
        
        _windowSensor = new WindowSensor
        {
            Id = 2, 
            Name = "Sensor de ventana", 
            Model = 456, 
            Description = "Sensor para ventanas"
        };
    }
    
    private void SetUpDefaultDeviceUnits()
    {
        _windowSensorUnit = new DeviceUnit
        {
            Device = _windowSensor,
            HardwareId = new Guid(),
            IsConnected = true
        };
        
        _securityCameraUnit = new DeviceUnit
        {
            Device = _securityCamera,
            HardwareId = new Guid(),
            IsConnected = false
        };
    }

    private void SetUpDefaultHome()
    {
        _defaultHome = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
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
        _mockDeviceRepository = new Mock<IRepository<Device>>();
    }
}