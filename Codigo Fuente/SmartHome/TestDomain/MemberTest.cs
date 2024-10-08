using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class MemberTest
{
    private User _user;
    private Member _member;
    private Notification _notification;
    
    private string _notificationEvent = "New Notification";

    
    [TestInitialize]
    public void TestInitialize()
    {
        _user = new User(); 
        _member = new Member(_user);
        _notification = new Notification(_notificationEvent);
    }
    
    [TestMethod]
    public void TestAddNewMember()
    {
        Assert.AreEqual(_user, _member.User);
    }
    
    [TestMethod]
    public void TestAddReciveNotifications()
    {
        _member.ReceivesNotifications = true; 
        
        Assert.IsTrue(_member.ReceivesNotifications);
    }
    
    [TestMethod]
    public void TestAddHasPermissionToListDevices()
    {
        _member.HasPermissionToListDevices= true; 
        
        Assert.IsTrue(_member.HasPermissionToListDevices);
    }
    
    [TestMethod]
    public void TestAddHasPermissionToAddADevices()
    {
        _member.HasPermissionToAddADevice = true; 
        
        Assert.IsTrue(_member.HasPermissionToAddADevice);
    }
    
    [TestMethod]
    public void AddNotification()
    {
        _member.AddNotification(_notification);
        
        Assert.AreEqual(_member.Notifications.Count, 1);
    }
    
    [TestMethod]
    public void RemoveNotification()
    {
        _member.AddNotification(_notification);
        _member.RemoveNotification(_notification);
        
        Assert.AreEqual(_member.Notifications.Count, 0);
    }
    
    [TestMethod]
    public void GetNotificationById()
    {
        _member.AddNotification(_notification);
        
        Assert.AreEqual(_member.GetNotificationById(_notification.Id), _notification);
    }
    
    [TestMethod]
    public void CompareMembersById()
    {
        Member member = new Member(_user);
        member.Id = _member.Id + 1;
        
        Assert.AreNotEqual(_member.Id, member.Id);
    }
}