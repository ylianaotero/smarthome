using BusinessLogic;
using Domain.Concrete;
using Domain.Enum;
using IBusinessLogic;
using Moq;

namespace TestService;

[TestClass]
public class ActionServiceTest
{
    [TestMethod]
    public void TestPostAction()
    {
        Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
        Mock<IDeviceUnitService> deviceUnitServiceMock = new Mock<IDeviceUnitService>();
        
        ActionService actionService = new ActionService(notificationServiceMock.Object, deviceUnitServiceMock.Object);

        SmartLamp smartLamp = new SmartLamp()
        {
            Id = 1,
            Name = "Smart Lamp",
            Model = "SL-1",
            Description = "Smart Lamp",
            PhotoURLs = new List<string>() { "example.png" },
            Functionalities = new List<SmartLampFunctionality>() { SmartLampFunctionality.OnOff }
        };
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            HardwareId = Guid.NewGuid(),
            Device = smartLamp,
            Status = "Off"
        };

        User user = new User()
        {
            Id = 1,
            Password = "Password1#",
            Email = "user@gmail.com",
            Name = "User",
            Surname = "User",
        };
        
        Home home = new Home()
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
        
        string functionality = "OnOff";
        
        string result = actionService.PostAction(home.Id, deviceUnit.HardwareId, functionality);
        string expectedResult = "The functionality " + functionality + " has been executed in device " +
                                deviceUnit.HardwareId + " from home " + home.Id + ". Status has been changed to On";
        
        Assert.AreEqual(expectedResult, result);
    }
}