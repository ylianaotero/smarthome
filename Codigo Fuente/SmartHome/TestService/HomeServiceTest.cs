using BusinessLogic;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using Domain.DTO;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class HomeServiceTest
{
    private Mock<IRepository<Home>> _mockHomeRepository;
    private Mock<IRepository<Device>> _mockDeviceRepository;
    private Mock<IRepository<User>> _mockUserRepository;
    private Mock<IRepository<Member>> _mockMemberRepository;
    private Mock<IRepository<DeviceUnit>> _mockDeviceUnitRepository;
    
    private HomeService _homeService;
    private Home _defaultHome;
    private WindowSensor _windowSensor;
    private SecurityCamera _securityCamera;
    private DeviceUnit _windowSensorUnit;
    private DeviceUnit _securityCameraUnit;
    private DeviceUnit _updatedDevice;
    private User _user1;
    private User _user2;
    private User _defaultOwner;
    private MemberDTO _memberDTO2;
    private MemberDTO _memberDTO1; 
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
    private const bool IsConnectedTrue = true;
    private const bool IsConnectedFalse = false;
    private const string RoomName = "Living Room";

    
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
    public void TestAddMemberToHome()
    {
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<User>() {_user1});
        
        _homeService.AddMemberToHome(_defaultHome.Id, _memberDTO1);
        
        _mockHomeRepository.Verify(m => m.Update(_defaultHome), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestCannotAddMemberToHomeBecauseItIsAlreadyExist()
    {
        _defaultHome.AddMember(_member1);
    
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);

        HomeService homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object, 
            _mockUserRepository.Object, _mockMemberRepository.Object, _mockDeviceUnitRepository.Object);

        homeService.AddMemberToHome(_defaultHome.Id, _memberDTO1);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotAddMemberToHomeBecauseItIsNotFound()
    {
        int nonExistentHomeId = Id;
    
        _mockHomeRepository.Setup(m => m.GetById(nonExistentHomeId)).Returns((Home)null);
        
        _homeService.AddMemberToHome(nonExistentHomeId, _memberDTO1);
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
    public void TestChangePermission()
    {
        MemberDTO memberDto = new MemberDTO()
        {
            UserEmail = _user1.Email,
            ReceivesNotifications = true
        }; 
        
        _defaultHome.AddMember(_member1);
        
        _mockHomeRepository
            .Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        _homeService.UpdateMemberNotificationPermission(memberDto, _defaultHome.Id);
        
        Assert.AreEqual(memberDto.ReceivesNotifications, _defaultHome.FindMember(memberDto.UserEmail).ReceivesNotifications);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotChangePermissionBecauseHomeNotFound()
    {
        MemberDTO memberDto = new MemberDTO()
        {
            UserEmail = _user1.Email,
            ReceivesNotifications = true
        }; 
        
        _defaultHome.AddMember(_member1);
        
        _mockHomeRepository
            .Setup(m => m.GetById(_defaultHome.Id)).Returns((Home?)null);
        
        _homeService.UpdateMemberNotificationPermission(memberDto, _defaultHome.Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotChangePermissionBecauseMemberNotFound()
    {
        MemberDTO memberDto = new MemberDTO()
        {
            UserEmail = _user1.Email,
            ReceivesNotifications = true
        }; 
        
        _mockHomeRepository
            .Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        _homeService.UpdateMemberNotificationPermission(memberDto, _defaultHome.Id);
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
    public void TestPutDevicesInHome()
    {
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns(_defaultHome);
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
        
        _homeService.AddDevicesToHome(1, homeDevicesDTO);
       
        CollectionAssert.AreEqual(_defaultHome.Devices,homeDevices);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestPutDevicesInHomeThrowsElementNotFound()
    {
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns((Home?)null);
       
        List<DeviceUnitDTO> homeDevicesDTO = new List<DeviceUnitDTO> {};
        
        _homeService.AddDevicesToHome(1, homeDevicesDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestPutDevicesInHomeThrowsElementNotFoundBecauseOfDevice()
    {
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns(_defaultHome);
        _mockDeviceRepository.Setup(x=>x.GetById(_securityCamera.Id)).Returns((Device?)null);
        
        List<DeviceUnitDTO> homeDevicesDTO = new List<DeviceUnitDTO>
        {
            new DeviceUnitDTO
            {
                DeviceId = _securityCamera.Id,
                IsConnected = _securityCameraUnit.IsConnected
            }
        };
        
        _homeService.AddDevicesToHome(1, homeDevicesDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestAddMemberToHomeFailsIfMaxMembersHasBeenReached()
    {
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<User>() {_user1, _user2});
        
        _homeService.AddMemberToHome(_defaultHome.Id, _memberDTO1);
        _homeService.AddMemberToHome(_defaultHome.Id, _memberDTO2);
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddMemberToHomeFailsIfUserNotFound()
    {
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<User>());
        
        _homeService.AddMemberToHome(_defaultHome.Id, _memberDTO1);
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
        _mockUserRepository.Setup(m => m.GetById(homeOwner.Id)).Returns(homeOwner);
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        Home homeWithOwner = _homeService.AddOwnerToHome(homeOwner.Id, _defaultHome);
        
        Assert.AreEqual(homeOwner, homeWithOwner.Owner);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddUnexistentOwnerToHome()
    {
        HomeOwner role = new HomeOwner();
        User homeOwner = new User()
        {
            Email = NewEmail,
            Id = 1,
            Roles = new List<Role>(){role},
        };
        
        _defaultHome.Owner = null;
        
        _mockUserRepository.Setup(m => m.GetById(homeOwner.Id)).Returns((User?)null);
        
        Home homeWithOwner = _homeService.AddOwnerToHome(homeOwner.Id, _defaultHome);
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
        _mockUserRepository.Setup(m => m.GetById(user.Id)).Returns(user);
        
        _homeService.AddOwnerToHome(user.Id, _defaultHome);
    }
    
    [TestMethod]
    public void TestUpdateDeviceConnectionStatus()
    {
        DeviceUnit device = new DeviceUnit()
        {
            Device = _securityCamera,
            HardwareId = Guid.NewGuid(),
            IsConnected = IsConnectedTrue
        };

        _defaultHome.Devices = new List<DeviceUnit>()
        {
            device
        };
        
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        _mockDeviceRepository.Setup(m => m.GetById(device.Device.Id)).Returns(device.Device);
        
        _homeService.UpdateDeviceConnectionStatus(_defaultHome.Id, _updatedDevice);
        
        _mockHomeRepository.Verify(m => m.Update(_defaultHome), Times.Once);
        
        Assert.AreEqual(IsConnectedFalse, _defaultHome.Devices[0].IsConnected);
    }
    
    [TestMethod] 
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateDeviceStatusBecauseHomeNotFound()
    {
        _securityCameraUnit.IsConnected = IsConnectedTrue;

        _defaultHome.Devices = new List<DeviceUnit>()
        {
            _securityCameraUnit
        };
        
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns((Home?)null);
        
        _homeService.UpdateDeviceConnectionStatus(_defaultHome.Id, _updatedDevice);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateDeviceStatusBecauseDeviceNotFound()
    {
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        _mockDeviceRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns((Device?)null);
        
        _homeService.UpdateDeviceConnectionStatus(_defaultHome.Id, _updatedDevice);
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
    public void TestUpdateCustomDeviceName()
    {
        _defaultHome.Devices = new List<DeviceUnit>()
        {
            _securityCameraUnit
        };
        
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        
        _homeService.UpdateDeviceCustomName(_defaultHome.Id, _updatedDevice, _securityCameraUnit.HardwareId);
        
        Assert.AreEqual(_updatedDevice, _defaultHome.Devices[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateCustomDeviceNameBecauseHomeNotFound()
    {
        _defaultHome.Devices = new List<DeviceUnit>();
        
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns((Home?)null);
        
        _homeService.UpdateDeviceCustomName(_defaultHome.Id, _updatedDevice, _securityCameraUnit.HardwareId);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateCustomDeviceNameBecauseDeviceUnitNotFound()
    {
        _mockHomeRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns((Home?)null);
        
        _homeService.UpdateDeviceCustomName(_defaultHome.Id, _updatedDevice, Guid.NewGuid());
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
    
    private void SetupDefaultObjects()
    {
        _homeService = new HomeService(_mockHomeRepository.Object, _mockDeviceRepository.Object,
            _mockUserRepository.Object, _mockMemberRepository.Object, _mockDeviceUnitRepository.Object);
        
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
        
         _updatedDevice = new DeviceUnit()
        {
            Device = _securityCamera,
            HardwareId = Guid.NewGuid(),
            IsConnected = IsConnectedFalse
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
        _memberDTO1 = new MemberDTO() { UserEmail = NewEmail };
        _memberDTO2 = new MemberDTO() { UserEmail = NewEmail2 }; 
    }

    private void CreateRepositoryMocks()
    {
        _mockHomeRepository = new Mock<IRepository<Home>>();
        _mockDeviceRepository = new Mock<IRepository<Device>>();
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockMemberRepository = new Mock<IRepository<Member>>();
        _mockDeviceUnitRepository = new Mock<IRepository<DeviceUnit>>();
    }
}