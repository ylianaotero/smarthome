using Domain;

namespace TestDomain;

[TestClass]
public class NotificationTest
{
    private string _event = "Door Opened";
    private DateTime _todayDate = DateTime.Today.Date;
    private bool _read = false;
    private DateTime _minValueDate = DateTime.MinValue;

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
        Assert.AreEqual(_todayDate, notification.CreatedAt.Date);
        Assert.AreEqual(_minValueDate, notification.ReadAt);
    }
    
    [TestMethod]
    public void ReadNotification()
    {
        // Act
        _notification.MarkAsRead(); 

        // Assert
        Assert.AreEqual(true, _notification.Read);
        Assert.AreEqual(_todayDate, _notification.ReadAt.Date);
    }
}