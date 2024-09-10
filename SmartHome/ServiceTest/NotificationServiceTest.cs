namespace ServiceTest;

[TestClass]
public class NotificationServiceTest
{
    [TestMethod]
    public void createNotificationService()
    {
        // Arrange
        Notification notification = new Notification(_event);

        // Assert
        Assert.AreEqual(_event, notification.Event);
        Assert.AreEqual(_read, notification.Read);
        Assert.AreEqual(_createdAt, notification.CreatedAt.Date);
        Assert.AreEqual(_readAt, notification.ReadAt);
    }
}