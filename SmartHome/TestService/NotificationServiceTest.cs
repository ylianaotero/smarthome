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
    
    private const string _eventName = "New event";
    
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
        Notification newNotification = new Notification(_eventName);
        List<Notification> notifications = new List<Notification> { newNotification };
        _mockNotificationRepository
            .Setup(m => m.GetByFilter(It.IsAny<Func<Notification, bool>>(), null))
            .Returns(notifications);
        
        List<Notification> retrievedNotifications = _notificationService
            .GetNotificationsByFilter(n => n.Event == _eventName, null);
        
        Assert.AreEqual(notifications, retrievedNotifications);
    }
}