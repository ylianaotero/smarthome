using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class NotificationTest
{
    private const string Event = "Door Opened";
    private const int Id = 1;
    private readonly DateTime _todayDate = DateTime.Today.Date;
    private const bool Read = false;
    private readonly DateTime _minValueDate = DateTime.MinValue;
    private DeviceUnit _deviceUnit;
    private User _user;
    private Member _member;
    private Home _home;

    private Notification _notification; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _notification = new Notification(Event);
        _deviceUnit = new DeviceUnit();
        _user = new User();
        _member = new Member(_user);
        _home = new Home();
    }
    
    [TestMethod]
    public void CreateNewNotification()
    {
        Notification notification = new Notification(Event);
        notification.Id = Id; 
        notification.Home = _home;
        notification.Member = _member;
        notification.DeviceUnit = _deviceUnit;

        Assert.IsTrue(
            Event == notification.Event &&
            Read == notification.Read &&
            _todayDate == notification.CreatedAt.Date &&
            _minValueDate == notification.ReadAt &&
            Id == notification.Id && _home == notification.Home && _member == notification.Member &&
            _deviceUnit.Device == notification.DeviceUnit.Device
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