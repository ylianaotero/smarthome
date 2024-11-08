using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class NotificationTest
{
    private User _user;
    private Home _home;
    private Member _member;
    private DeviceUnit _deviceUnitService;
    private Notification _notification; 

    
    private const int Id = 1;
    private const bool Read = false;
    private const string Event = "Door Opened";
    private readonly DateTime _todayDate = DateTime.Today.Date;
    private readonly DateTime _minValueDate = DateTime.MinValue;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _notification = new Notification(Event);
        _deviceUnitService = new DeviceUnit();
        _user = new User();
        _member = new Member(_user);
        _home = new Home();
    }
    
    [TestMethod]
    public void CreateNewNotification()
    {
        _notification.Id = Id; 
        _notification.Home = _home;
        _notification.Member = _member;
        _notification.DeviceUnitService = _deviceUnitService;

        Assert.IsTrue(
            Event == _notification.Event &&
            Read == _notification.Read &&
            _todayDate == _notification.CreatedAt.Date &&
            _minValueDate == _notification.ReadAt &&
            Id == _notification.Id && _home == _notification.Home && _member == _notification.Member &&
            _deviceUnitService.Device == _notification.DeviceUnitService.Device
        );

    }
    
    [TestMethod]
    public void ReadNotification()
    {
        _notification.MarkAsRead(); 
        
        Assert.IsTrue(
            _notification.Read  &&
            _todayDate == _notification.ReadAt.Value.Date
        );
    }
    
    [TestMethod]
    public void DefaultEvent()
    {
        Notification notification = new Notification();
        
        Assert.IsTrue(notification.Event == "");
    }
}