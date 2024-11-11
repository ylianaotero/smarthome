using BusinessLogic;
using Domain.Concrete;
using Domain.DTO;
using Domain.Enum;
using IBusinessLogic;
using Moq;

namespace TestService;

[TestClass]
public class ActionServiceTest
{
    private Mock<INotificationService> notificationServiceMock;
    private Mock<IDeviceUnitService> deviceUnitServiceMock;

    private SmartLamp smartLamp;
    private DeviceUnit deviceUnit;
    private User user;
    private Home home;
        
        
    [TestInitialize]
    public void TestInitialize()
    {
        notificationServiceMock = new Mock<INotificationService>();
        deviceUnitServiceMock = new Mock<IDeviceUnitService>();
        SetupDefaultObjects();
    }
    
    [TestMethod]
    public void TestPostAction()
    {
        ActionService actionService = new ActionService(notificationServiceMock.Object, deviceUnitServiceMock.Object);
       
        string functionality = "OnOff";
        
        notificationServiceMock
            .Setup(n => n.SendNotifications(It.IsAny<NotificationDTO>()));
        deviceUnitServiceMock
            .Setup(d => d.ExecuteFunctionality(deviceUnit.HardwareId, functionality));
        
        string result = actionService.PostAction(home.Id, deviceUnit.HardwareId, functionality);
        string expectedResult = "The functionality " + functionality + " has been executed in device " +
                                deviceUnit.HardwareId + " from home " + home.Id + ".";
        
        Assert.AreEqual(expectedResult, result);
    }
    
    private void SetupDefaultObjects()
    {
        SetupDefaultDevices();
        
        user = new User()
        {
            Id = 1,
            Password = "Password1#",
            Email = "user@gmail.com",
            Name = "User",
            Surname = "User",
        };
        
        home = new Home()
        {
            Id = 1,
            Devices = new List<DeviceUnit>() { deviceUnit },
            Members = new List<Member>() { new Member() { User = user, ReceivesNotifications = true } },
            Alias = "Home",
            DoorNumber = 12,
            Street = "Street",
            Longitude = 1234.56,
            Latitude = 1234.56,
            MaximumMembers = 2,
        };
    }
    
    private void SetupDefaultDevices()
    {
        smartLamp = new SmartLamp()
        {
            Id = 1,
            Name = "Smart Lamp",
            Model = "SL-1",
            Description = "Smart Lamp",
            PhotoURLs = new List<string>() { "example.png" },
            Functionalities = new List<SmartLampFunctionality>() { SmartLampFunctionality.OnOff }
        };
        
        deviceUnit = new DeviceUnit()
        {
            HardwareId = Guid.NewGuid(),
            Device = smartLamp,
            Status = "Off"
        };
    }
}