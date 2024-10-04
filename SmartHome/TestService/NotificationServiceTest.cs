using BusinessLogic;
using CustomExceptions;
using Domain;
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
        _notificationService = new NotificationService(_mockNotificationRepository.Object);
    }
    
    [TestMethod]
    public void TestGetNotificationById()
    {
        int id = 1;
        Notification newNotification = new Notification("New event");
        _mockNotificationRepository.Setup(m => m.GetById(id)).Returns(newNotification);
        Notification retrievedNotification = _notificationService.GetNotificationById(id);
        Assert.AreEqual(newNotification, retrievedNotification);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestGetNotificationByIdNotFound()
    {
        int id = 1;
        _mockNotificationRepository.Setup(m => m.GetById(id)).Returns((Notification)null);
        _notificationService.GetNotificationById(id);
    }

    [TestMethod]
    public void TestGetNotificationsByFilter()
    {
        string eventName = "New event";
        Notification newNotification = new Notification(eventName);
        List<Notification> notifications = new List<Notification> { newNotification };
        _mockNotificationRepository.Setup(m => m.GetByFilter(It.IsAny<Func<Notification, bool>>(), null)).Returns(notifications);
        List<Notification> retrievedNotifications = _notificationService.GetNotificationsByFilter(n => n.Event == eventName, null);
        Assert.AreEqual(notifications, retrievedNotifications);
    }
}