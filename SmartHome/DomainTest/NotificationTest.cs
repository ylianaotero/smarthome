using Domain;

namespace DomainTest;

[TestClass]
public class NotificationTest
{
    private string _event = "Door Opened";
    private DateTime _createdAt = DateTime.Today.Date;
    private bool _read = false;
    private DateTime _readAt = DateTime.MinValue;

    private Notification _notification; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _notification = new Notification(_event);
    }
    [TestMethod]
    public void CreateNewNotification()
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