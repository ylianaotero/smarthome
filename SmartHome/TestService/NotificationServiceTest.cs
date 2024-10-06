using BusinessLogic;
using Domain.Concrete;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class NotificationServiceTest
{
    private Mock<IRepository<Notification>> _mockNotificationRepository;
    private NotificationService _notificationService;
    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateMockNotificationRepository();
    }
    
    private void CreateMockNotificationRepository()
    {
        _mockNotificationRepository = new Mock<IRepository<Notification>>();
        Mock<IRepository<Home>> mockHomeRepository = new Mock<IRepository<Home>>();
        _notificationService = new NotificationService(_mockNotificationRepository.Object, mockHomeRepository.Object);
    }

    [TestMethod]
    public void TestGetNotificationsByFilter()
    {
        string eventName = "New event";
        Notification newNotification = new Notification(eventName);
        List<Notification> notifications = new List<Notification> { newNotification };
        _mockNotificationRepository
            .Setup(m => m.GetByFilter(It.IsAny<Func<Notification, bool>>(), null))
            .Returns(notifications);
        
        List<Notification> retrievedNotifications = _notificationService
            .GetNotificationsByFilter(n => n.Event == eventName, null);
        
        Assert.AreEqual(notifications, retrievedNotifications);
    }
}