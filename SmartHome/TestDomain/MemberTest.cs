using Domain;
using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class MemberTest
{
    private string _notificationEvent = "New Notification";
    private User _user; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _user = new User(); 
    }
    
    [TestMethod]
    public void TestAddNewMember()
    {
        Member member = new Member(_user);
        
        Assert.AreEqual(_user, member.User);
    }
    
    [TestMethod]
    public void TestAddReciveNotifications()
    {
        Member member = new Member(_user);

        member.ReceivesNotifications = true; 
        
        Assert.IsTrue(member.ReceivesNotifications);
    }
    
    [TestMethod]
    public void TestAddHasPermissionToListDevices()
    {
        Member member = new Member(_user);

        member.HasPermissionToListDevices= true; 
        
        Assert.IsTrue(member.HasPermissionToListDevices);
    }
    
    [TestMethod]
    public void TestAddHasPermissionToAddADevices()
    {
        Member member = new Member(_user);

        member.HasPermissionToAddADevice = true; 
        
        Assert.IsTrue(member.HasPermissionToAddADevice);
    }
    
    [TestMethod]
    public void AddNotification()
    {
        Member member = new Member(_user);
        Notification notification = new Notification(_notificationEvent);

        member.AddNotification(notification);
        
        Assert.AreEqual(member.Notifications.Count, 1);
    }
    
    [TestMethod]
    public void RemoveNotification()
    {
        Member member = new Member(_user);
        Notification notification = new Notification(_notificationEvent);

        member.AddNotification(notification);
        member.RemoveNotification(notification);
        
        Assert.AreEqual(member.Notifications.Count, 0);
    }
    
    [TestMethod]
    public void GetNotificationById()
    {
        Member member = new Member(_user);
        Notification notification = new Notification(_notificationEvent);

        member.AddNotification(notification);
        
        Assert.AreEqual(member.GetNotificationById(notification.Id), notification);
    }
    
    
    
}