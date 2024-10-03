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
    public void TestCreateNotification()
    {
        string eventName = "New event";
        Notification newNotification = new Notification(eventName);
        _mockNotificationRepository.Setup(m => m.Add(newNotification));
        NotificationService notificationService = new NotificationService(_mockNotificationRepository.Object);
        notificationService.CreateNotification(newNotification);
        _mockNotificationRepository.Verify(m => m.Add(newNotification), Times.Once);
    }

    [TestMethod]
    public void TestGetNotifications()
    {
        List<Notification> notifications = new List<Notification>();
        Notification newNotification = new Notification("New event");
        notifications.Add(newNotification);
        _mockNotificationRepository.Setup(m => m.GetAll()).Returns(notifications);
        List<Notification> retrievedNotifications = _notificationService.GetNotifications();
        Assert.AreEqual(notifications, retrievedNotifications);
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
}