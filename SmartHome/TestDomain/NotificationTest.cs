using Domain;

namespace TestDomain;

[TestClass]
public class NotificationTest
{
    private const string Event = "Door Opened";
    private const int Id = 1;
    private readonly DateTime _todayDate = DateTime.Today.Date;
    private const bool Read = false;
    private readonly DateTime _minValueDate = DateTime.MinValue;

    private Notification _notification; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _notification = new Notification(Event);
    }
    
    [TestMethod]
    public void CreateNewNotification()
    {
        Notification notification = new Notification(Event);
        notification.Id = Id; 
        
        Assert.IsTrue(
            Event == notification.Event &&
            Read == notification.Read &&
            _todayDate == notification.CreatedAt.Date &&
            _minValueDate == notification.ReadAt &&
            Id == notification.Id
        );

    }
    
    [TestMethod]
    public void ReadNotification()
    {
        _notification.MarkAsRead(); 
        
        Assert.IsTrue(
            _notification.Read  &&
            _todayDate == _notification.ReadAt.Date
        );

    }
}