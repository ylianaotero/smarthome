using BusinessLogic;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class DeviceUnitTest
{
    private Mock<IRepository<DeviceUnit>> _mockDeviceUnitRepository;
    private Mock<IHomeService> _mockHomeService;
    
    private IDeviceUnitService _deviceUnitService;
    private Home _defaultHome;
    private SecurityCamera _securityCamera;
    private DeviceUnit _securityCameraUnit;
    private DeviceUnit _updatedDevice;
    private User _defaultOwner;
    
    private const string Alias = "My Home";
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    private const string NewEmail = "juan.perez@example.com";
    private const int MaxMembers = 10;
    private const int Id = 999;
    private const bool IsConnectedTrue = true;
    private const bool IsConnectedFalse = false;
    private const string RoomName = "Living Room";
    private const string DeviceName = "Security Camera";
    private const string HomeNotFoundMessage = "Home not found";
    private const string RoomNotFoundMessage = "Room not found";
    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateMocks();
        SetupDefaultObjects();
    }
    
    [TestMethod]
    public void TestUpdateDeviceConnectionStatus()
    {
        DeviceUnit device = new DeviceUnit()
        {
            Device = _securityCamera,
            HardwareId = _updatedDevice.HardwareId,
            IsConnected = IsConnectedTrue
        };

        _defaultHome.Devices = [device];
        
        _mockHomeService.Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns(_defaultHome.Devices);
        
        _deviceUnitService.UpdateDeviceConnectionStatus(_defaultHome.Id, _updatedDevice);
        
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
        
        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id))
            .Throws(new ElementNotFound(HomeNotFoundMessage));
        
        _deviceUnitService.UpdateDeviceConnectionStatus(_defaultHome.Id, _updatedDevice);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateDeviceStatusBecauseDeviceNotFound()
    {
        _mockHomeService.Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        
        _deviceUnitService.UpdateDeviceConnectionStatus(_defaultHome.Id, _updatedDevice);
    }
    
    [TestMethod]
    public void TestUpdateCustomDeviceName()
    {
        _defaultHome.Devices = new List<DeviceUnit>()
        {
            _securityCameraUnit
        };
        
        _mockHomeService.Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns(_defaultHome.Devices);
        
        _deviceUnitService.UpdateDeviceCustomName(_defaultHome.Id, _updatedDevice, _securityCameraUnit.HardwareId);
        
        Assert.AreEqual(_updatedDevice, _defaultHome.Devices[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateCustomDeviceNameBecauseHomeNotFound()
    {
        _defaultHome.Devices = new List<DeviceUnit>();
        
        _mockHomeService.Setup(m => m.GetHomeById(_defaultHome.Id)).Returns((Home?)null);
        
        _deviceUnitService.UpdateDeviceCustomName(_defaultHome.Id, _updatedDevice, _securityCameraUnit.HardwareId);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateCustomDeviceNameBecauseDeviceUnitNotFound()
    {
        _mockHomeService.Setup(m => m.GetHomeById(It.IsAny<long>())).Returns((Home?)null);
        
        _deviceUnitService.UpdateDeviceCustomName(_defaultHome.Id, _updatedDevice, Guid.NewGuid());
    }
    
    [TestMethod]
    public void TestUpdateDeviceRoomInHome()
    {
        Room room = new Room()
        {
            Id = Id,
            Name = RoomName
        };

        _defaultHome.Rooms = [room];
        
        _defaultHome.Devices = [_securityCameraUnit];
        
        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns(_defaultHome.Devices);
        
        _mockHomeService
            .Setup(m => m.GetRoomsFromHome(_defaultHome.Id)).Returns(_defaultHome.Rooms);
        
        _mockHomeService
            .Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        
        _deviceUnitService.UpdateDeviceRoom(_defaultHome.Id, _securityCameraUnit, room);
        
        Assert.AreEqual(room, _defaultHome.Rooms[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestUpdateDeviceRoomInNonexistentHome()
    {
        Room room = new Room()
        {
            Id = Id,
            Name = RoomName
        };

        _defaultHome.Rooms = [room];
        
        _defaultHome.Devices = [_securityCameraUnit];

        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns((List<DeviceUnit>)null);
        
        _deviceUnitService.UpdateDeviceRoom(_defaultHome.Id, _securityCameraUnit, room);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestUpdateDeviceRoomInHomeThatDoesNotHaveDevice()
    {
        Room room = new Room()
        {
            Id = Id,
            Name = RoomName
        };

        _defaultHome.Rooms = [room];
        
        _defaultHome.Devices = [];
        
        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns(_defaultHome.Devices);
        
        _mockHomeService
            .Setup(m => m.GetRoomsFromHome(_defaultHome.Id)).Returns(_defaultHome.Rooms);
        
        _mockHomeService
            .Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        
        _deviceUnitService.UpdateDeviceRoom(_defaultHome.Id, _securityCameraUnit, room);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TestUpdateDeviceRoomInHomeThatDoesNotHaveRoom()
    {
        Room room = new Room()
        {
            Id = Id,
            Name = RoomName
        };

        _defaultHome.Rooms = [];
        
        _defaultHome.Devices = [_securityCameraUnit];

        _mockHomeService
            .Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        
        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns(_defaultHome.Devices);
        
        _mockHomeService
            .Setup(m => m.GetRoomsFromHome(_defaultHome.Id))
            .Throws(new CannotFindItemInList(RoomNotFoundMessage));
        
        _deviceUnitService.UpdateDeviceRoom(_defaultHome.Id, _securityCameraUnit, room);
    }
    
    private void CreateMocks()
    {
        _mockDeviceUnitRepository = new Mock<IRepository<DeviceUnit>>();
        _mockHomeService = new Mock<IHomeService>();
    }
    
    private void SetupDefaultObjects()
    {
        _deviceUnitService = new DeviceUnitService(_mockDeviceUnitRepository.Object, _mockHomeService.Object);
        SetUpDefaultDeviceUnits();
        SetUpDefaultHome();
    }
    
    private void SetUpDefaultDeviceUnits()
    {
        _securityCameraUnit = new DeviceUnit
        {
            Device = _securityCamera,
            HardwareId = Guid.NewGuid(),
            IsConnected = false
        };
        
        _updatedDevice = new DeviceUnit()
        {
            Name = DeviceName,
            Device = _securityCamera,
            HardwareId = _securityCameraUnit.HardwareId,
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
}