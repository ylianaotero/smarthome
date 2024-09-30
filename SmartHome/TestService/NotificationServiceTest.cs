using BusinessLogic;
using Domain;
using Moq;

namespace TestService;

[TestClass]
public class NotificationServiceTest
{
    private Mock<Notification> _mockNotificationRepository;
    private NotificationService _notificationService;
    
    [TestMethod]
    public void TestCreateNotification()
    {
        string newEvent = "New event";
        Notification newNotification = new Notification(newEvent);
        
        _mockNotificationRepository.Setup(n => n.Add(newNotification));
        
        _notificationService.CreateNotification(newEvent);
        
        _mockNotificationRepository.Verify(x => x.Add(newNotification), Times.Once);
    }
}