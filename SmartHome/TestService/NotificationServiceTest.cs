using BusinessLogic;
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
}