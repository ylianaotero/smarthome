using BusinessLogic;
using Domain.Concrete;
using Domain.DTO;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class ActionServiceTest
{
    [TestMethod]
    public void TestExecuteActionGeneratesNotification()
    {
        var homeRepositoryMock = new Mock<IRepository<Home>>();
        var notificationServiceMock = new Mock<INotificationService>();
        var deviceUnitServiceMock = new Mock<IDeviceUnitService>();
        
        ActionService actionService = new ActionService(notificationServiceMock.Object, deviceUnitServiceMock.Object);
        Guid hardwareid = Guid.NewGuid();
        
        var notificationData = new NotificationDTO
        {
            HomeId = 1,
            HardwareId = hardwareid,
            Event = "TestEvent"
        };

        string action = "HumanDetection"; 
        
        var home = new Home
        {
            Id = 1,
            Devices = new List<DeviceUnit>
            {
                new DeviceUnit
                {
                    HardwareId = hardwareid,
                    IsConnected = true
                }
            },
            Members = new List<Member>
            {
                new Member
                {
                    ReceivesNotifications = true,
                    Notifications = new List<Notification>()
                }
            }
        };
        
        homeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(home);
        
        Assert.AreEqual(1, home.Members[0].Notifications.Count);
    }
}