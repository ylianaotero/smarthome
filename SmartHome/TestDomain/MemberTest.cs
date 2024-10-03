using Domain;

namespace TestDomain;

[TestClass]
public class MemberTest
{
    private string _notificationEvent = "New Notification";
    
    [TestMethod]
    public void CreateNewHome()
    {
        Member member = new Member();

        member.Permission = true; 
        
        Assert.IsTrue(member.Permission);
    }
    
    [TestMethod]
    public void CreateNewMember()
    {
        Member member = new Member();

        member.ReceivesNotifications = true; 
        
        Assert.IsTrue(member.ReceivesNotifications);
    }
    
    [TestMethod]
    public void AddNotification()
    {
        Member member = new Member();
        Notification notification = new Notification(_notificationEvent);

        member.AddNotification(notification);
        
        Assert.AreEqual(member.GetAllNotifications().Count, 1);
    }
    
    [TestMethod]
    public void RemoveNotification()
    {
        Member member = new Member();
        Notification notification = new Notification(_notificationEvent);

        member.AddNotification(notification);
        member.RemoveNotification(notification);
        
        Assert.AreEqual(member.GetAllNotifications().Count, 0);
    }
    
    [TestMethod]
    public void GetNotificationById()
    {
        Member member = new Member();
        Notification notification = new Notification(_notificationEvent);

        member.AddNotification(notification);
        
        Assert.AreEqual(member.GetNotificationById(notification.Id), notification);
    }
    
    
    
}