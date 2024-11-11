using System.Linq.Expressions;
using BusinessLogic;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using Domain.DTO;
using Domain.Enum;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class DeviceUnitServiceTest
{
    private Mock<IRepository<DeviceUnit>> _mockDeviceUnitRepository;
    private Mock<IHomeService> _mockHomeService;
    private Mock<IDeviceService> _mockDeviceService;
    
    private IDeviceUnitService _deviceUnitService;
    private Home _defaultHome;
    private SecurityCamera _securityCamera;
    private WindowSensor _windowSensor;
    private DeviceUnit _securityCameraUnit;
    private DeviceUnit _windowSensorUnit;
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
    private const string DeviceNotFoundMessage = "Device not found";
    
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
        
        DeviceUnitDTO deviceUnitDto = new DeviceUnitDTO()
        {
            HardwareId = device.HardwareId,
            IsConnected = !device.IsConnected,
        };
        
        _defaultHome.Devices = [device];
        
        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns(_defaultHome.Devices);
        
        _deviceUnitService.UpdateDeviceUnit(_defaultHome.Id, deviceUnitDto);
        
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
        
        DeviceUnitDTO deviceUnitDto = new DeviceUnitDTO()
        {
            HardwareId = _securityCameraUnit.HardwareId,
            IsConnected = !_securityCameraUnit.IsConnected
        };
        
        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id))
            .Throws(new ElementNotFound(HomeNotFoundMessage));
        
        _deviceUnitService.UpdateDeviceUnit(_defaultHome.Id, deviceUnitDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateDeviceStatusBecauseDeviceNotFound()
    {
        _mockHomeService.Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        
        DeviceUnitDTO deviceUnitDto = new DeviceUnitDTO()
        {
            HardwareId = _updatedDevice.HardwareId,
            IsConnected = !_updatedDevice.IsConnected
        };
        
        _deviceUnitService.UpdateDeviceUnit(_defaultHome.Id, deviceUnitDto);
    }
    
    [TestMethod]
    public void TestUpdateCustomDeviceName()
    {
        _defaultHome.Devices = new List<DeviceUnit>()
        {
            _securityCameraUnit
        };
        
        DeviceUnitDTO deviceUnitDto = new DeviceUnitDTO()
        {
            HardwareId = _securityCameraUnit.HardwareId,
            Name = _updatedDevice.Name
        };
        
        _mockHomeService.Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns(_defaultHome.Devices);
        
        _deviceUnitService.UpdateDeviceUnit(_defaultHome.Id, deviceUnitDto);
        
        Assert.AreEqual(_updatedDevice, _defaultHome.Devices[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateCustomDeviceNameBecauseHomeNotFound()
    {
        _defaultHome.Devices = new List<DeviceUnit>();
        
        DeviceUnitDTO deviceUnitDto = new DeviceUnitDTO()
        {
            HardwareId = _securityCameraUnit.HardwareId,
            Name = _updatedDevice.Name
        };
        
        _mockHomeService.Setup(m => m.GetHomeById(_defaultHome.Id)).Returns((Home?)null);
        
        _deviceUnitService.UpdateDeviceUnit(_defaultHome.Id, deviceUnitDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotUpdateCustomDeviceNameBecauseDeviceUnitNotFound()
    {
        _mockHomeService.Setup(m => m.GetHomeById(It.IsAny<long>())).Returns((Home?)null);
        
        DeviceUnitDTO deviceUnitDto = new DeviceUnitDTO()
        {
            HardwareId = Guid.NewGuid(),
            Name = _updatedDevice.Name
        };
        
        _deviceUnitService.UpdateDeviceUnit(_defaultHome.Id, deviceUnitDto);
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
        
        DeviceUnitDTO deviceUnitDto = new DeviceUnitDTO()
        {
            HardwareId = _securityCameraUnit.HardwareId,
            RoomId = room.Id
        };
        
        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns(_defaultHome.Devices);
        
        _mockHomeService
            .Setup(m => m.GetRoomsFromHome(_defaultHome.Id)).Returns(_defaultHome.Rooms);
        
        _mockHomeService
            .Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        
        _deviceUnitService.UpdateDeviceUnit(_defaultHome.Id, deviceUnitDto);
        
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
        
        DeviceUnitDTO deviceUnitDto = new DeviceUnitDTO()
        {
            HardwareId = _securityCameraUnit.HardwareId,
            RoomId = room.Id
        };

        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns((List<DeviceUnit>)null);
        
        _deviceUnitService.UpdateDeviceUnit(_defaultHome.Id, deviceUnitDto);
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
        
        DeviceUnitDTO deviceUnitDto = new DeviceUnitDTO()
        {
            HardwareId = _securityCameraUnit.HardwareId,
            RoomId = room.Id
        };
        
        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns(_defaultHome.Devices);
        
        _mockHomeService
            .Setup(m => m.GetRoomsFromHome(_defaultHome.Id)).Returns(_defaultHome.Rooms);
        
        _mockHomeService
            .Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        
        _deviceUnitService.UpdateDeviceUnit(_defaultHome.Id, deviceUnitDto);
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
        
        DeviceUnitDTO deviceUnitDto = new DeviceUnitDTO()
        {
            HardwareId = _securityCameraUnit.HardwareId,
            RoomId = room.Id
        };

        _mockHomeService
            .Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        
        _mockHomeService
            .Setup(m => m.GetDevicesFromHome(_defaultHome.Id)).Returns(_defaultHome.Devices);
        
        _mockHomeService
            .Setup(m => m.GetRoomsFromHome(_defaultHome.Id))
            .Throws(new CannotFindItemInList(RoomNotFoundMessage));
        
        _deviceUnitService.UpdateDeviceUnit(_defaultHome.Id, deviceUnitDto);
    }
    
    [TestMethod]
    public void TestAddDevicesToHome()
    {
        _mockHomeService.Setup(x=>x.GetHomeById(1)).Returns(_defaultHome);
        _mockDeviceService.Setup(x=>x.GetDeviceById(_securityCamera.Id)).Returns(_securityCamera);
        _mockDeviceService.Setup(x => x.GetDeviceById(_windowSensor.Id)).Returns(_windowSensor);
        _mockDeviceUnitRepository.Setup(x => x.Add(_securityCameraUnit));
        _mockDeviceUnitRepository.Setup(x => x.Add(_windowSensorUnit));
        _mockHomeService.Setup(x => x.UpdateHome(_defaultHome.Id));
        
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
        
        _deviceUnitService.AddDevicesToHome(1, homeDevicesDTO);
        
        _windowSensorUnit.HardwareId = _defaultHome.Devices[0].HardwareId;
        _securityCameraUnit.HardwareId = _defaultHome.Devices[1].HardwareId;
       
        CollectionAssert.AreEqual(_defaultHome.Devices,homeDevices);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddDevicesToHomeThrowsElementNotFound()
    {
        _mockHomeService
            .Setup(x => x.GetHomeById(1))
            .Throws(new ElementNotFound(HomeNotFoundMessage));
       
        List<DeviceUnitDTO> homeDevicesDTO = new List<DeviceUnitDTO> {};
        
        _deviceUnitService.AddDevicesToHome(1, homeDevicesDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddDevicesToHomeThrowsElementNotFoundBecauseOfDevice()
    {
        _mockHomeService.Setup(x=>x.GetHomeById(1)).Returns(_defaultHome);
        _mockDeviceService
            .Setup(x => x.GetDeviceById(_securityCamera.Id))
            .Throws(new ElementNotFound(DeviceNotFoundMessage));
        
        List<DeviceUnitDTO> homeDevicesDTO = new List<DeviceUnitDTO>
        {
            new DeviceUnitDTO
            {
                DeviceId = _securityCamera.Id,
                IsConnected = _securityCameraUnit.IsConnected
            }
        };
        
        _deviceUnitService.AddDevicesToHome(1, homeDevicesDTO);
    }

    [TestMethod] 
    public void TestExecuteFunctionality()
    {
        DeviceUnit device = new DeviceUnit()
        {
            Device = _windowSensor,
            HardwareId = _updatedDevice.HardwareId,
            IsConnected = IsConnectedTrue,
            Status = "Open"
        };
        
        _defaultHome.Devices = [device];

        string functionality = "OpenClosed";   
        Func<DeviceUnit, bool> filter = x => x.HardwareId == device.HardwareId;
        
        _mockDeviceUnitRepository
            .Setup(x => x
                .GetByFilter(It.IsAny<Func<DeviceUnit, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<DeviceUnit> {device});
        
        _deviceUnitService.ExecuteFunctionality(device.HardwareId, functionality);
        
        Assert.AreEqual("Closed", _defaultHome.Devices[0].Status);
    }
    
    private void CreateMocks()
    {
        _mockDeviceUnitRepository = new Mock<IRepository<DeviceUnit>>();
        _mockHomeService = new Mock<IHomeService>();
        _mockDeviceService = new Mock<IDeviceService>();
    }
    
    private void SetupDefaultObjects()
    {
        _deviceUnitService = new DeviceUnitService
            (_mockDeviceUnitRepository.Object, _mockDeviceService.Object, _mockHomeService.Object);
        SetUpDefaultDevices();
        SetUpDefaultDeviceUnits();
        SetUpDefaultHome();
    }
    
    private void SetUpDefaultDevices()
    {
        _securityCamera = new SecurityCamera
        {
            Id = 1, 
            Name = "Cámara de seguridad", 
            Model = "123", 
            Description = "Cámara para exteriores",
            Functionalities = new List<SecurityCameraFunctionality>() {SecurityCameraFunctionality.HumanDetection}
        };
        
        _windowSensor = new WindowSensor
        {
            Id = 2, 
            Name = "Sensor de ventana", 
            Model = "456", 
            Description = "Sensor para ventanas",
            Functionalities = new List<WindowSensorFunctionality>() { WindowSensorFunctionality.OpenClosed }
        };
    }
    
    private void SetUpDefaultDeviceUnits()
    {
        _securityCameraUnit = new DeviceUnit
        {
            Device = _securityCamera,
            HardwareId = Guid.NewGuid(),
            IsConnected = false
        };
        
        _windowSensorUnit = new DeviceUnit
        {
            Device = _windowSensor,
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