using BusinessLogic;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using Domain.DTO;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class NotificationServiceTest
{
    private Mock<IRepository<Notification>> _mockNotificationRepository;
    private NotificationService _notificationService;
    private Mock<IRepository<Home>> _mockHomeRepository; 
    
    private const string _eventName = "New event";
    
    //
    
    private Home _defaultHome;
    
    

    
    private WindowSensor _windowSensor;
    private SecurityCamera _securityCamera;
    private DeviceUnit _windowSensorUnit;
    private DeviceUnit _securityCameraUnit;
    private DeviceUnit _updatedDevice;
    
    private User _user1;
    private User _user2;
    private User _defaultOwner;
    private DeviceUnit _deviceUnit; 
    private Device _device; 
    
    private const string WindowSensorName = "My Window Sensor";
    private const string DeviceDescription = "This is a device";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const long DeviceModel = 1345354616346;
    private static readonly Guid Guid = new Guid();


    private Member _member2;
    private Member _member1; 
    
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    private const string NewEmail = "juan.perez@example.com";
    private const int MaxMembers = 10;
    
    private const string TestEvent = "TestEvent";
    private const long TestHomeId = 12345; 


    private NotificationDTO _notificationDto; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateMockNotificationRepository();
        SetUpDefaultHome();
        SetUpNotificationDTO();
    }

    private void SetUpNotificationDTO()
    {
        _notificationDto = new NotificationDTO()
        {
            Event = TestEvent,
            HardwareId = Guid,
            HomeId = TestHomeId
        };
    }
    
    private void SetUpDefaultHome()
    {
        _device = new WindowSensor()
        {
            Id = 1,
            Name = WindowSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl }
            
        };
        _deviceUnit = new DeviceUnit()
        {
            Device = _device,
            IsConnected = false,
            HardwareId = Guid
        }; 
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
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = MaxMembers,
            Devices = new List<DeviceUnit>() {_deviceUnit}
        };
    }
    
    private void CreateMockNotificationRepository()
    {
        _mockNotificationRepository = new Mock<IRepository<Notification>>();
        
        _mockHomeRepository = new Mock<IRepository<Home>>();
        _notificationService = new NotificationService(_mockNotificationRepository.Object, _mockHomeRepository.Object);
    }

    [TestMethod]
    public void TestGetNotificationsByFilter()
    {
        Notification newNotification = new Notification(_eventName);
        List<Notification> notifications = new List<Notification> { newNotification };
        _mockNotificationRepository
            .Setup(m => m.GetByFilter(It.IsAny<Func<Notification, bool>>(), null))
            .Returns(notifications);
        
        _notificationService = new NotificationService(_mockNotificationRepository.Object, _mockHomeRepository.Object);
        
        List<Notification> retrievedNotifications = _notificationService
            .GetNotificationsByFilter(n => n.Event == _eventName, null);
        
        Assert.AreEqual(notifications, retrievedNotifications);
    }
    
    [TestMethod]
    public void TestSendNotifications()
    {
        _mockHomeRepository
            .Setup(m => m.GetById(_notificationDto.HomeId))
            .Returns(_defaultHome);
        
        _notificationService = new NotificationService(_mockNotificationRepository.Object, _mockHomeRepository.Object);
        
        _notificationService.SendNotifications(_notificationDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestSendNotificationsHomeNotFound()
    {
        _mockHomeRepository
            .Setup(m => m.GetById(_notificationDto.HomeId))
            .Returns((Home?)null);
        
        _notificationService = new NotificationService(_mockNotificationRepository.Object, _mockHomeRepository.Object);
        
        _notificationService.SendNotifications(_notificationDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestSendNotificationsDevicesNotFound()
    {
        _defaultHome = new Home()
        {
            Id = 1,
            Owner = _defaultOwner,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = MaxMembers,
            Devices = new List<DeviceUnit>() {},
            Members = new List<Member>() {}
        };
        
        User user = new User(); 
        Member member = new Member(user);
        member.ReceivesNotifications = true; 
        
        _mockHomeRepository
            .Setup(m => m.GetById(_notificationDto.HomeId))
            .Returns((Home?)null);
        
        _notificationService = new NotificationService(_mockNotificationRepository.Object, _mockHomeRepository.Object);
        
        _notificationService.SendNotifications(_notificationDto);
    }
}