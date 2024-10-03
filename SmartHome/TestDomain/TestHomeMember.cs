using Domain;

namespace TestDomain;

[TestClass]
public class TestHomeMember
{
    private const long Id = 1345354616346;
    
    [TestMethod]
    public void TestAddMemberRole()
    {
        HomeMember homeMember = new HomeMember();
        homeMember.Id = Id;
        Assert.AreEqual(homeMember.Id, Id);
    }
    
    [TestMethod]
    public void TestAddHomeToMemberRole()
    {
        Home home = new Home();
        HomeMember homeMember = new HomeMember();
        homeMember.Home = home;
        Assert.AreEqual(homeMember.Home, home);
    }
    
    [TestMethod]
    public void TestAddHasPermissionToAddADeviceToMemberRole()
    {
        HomeMember homeMember = new HomeMember();
        homeMember.HasPermissionToAddADevice = true;
        Assert.IsTrue(homeMember.HasPermissionToAddADevice);
    }
    
    [TestMethod]
    public void TestAddReceivesNotificationsToMemberRole()
    {
        HomeMember homeMember = new HomeMember();
        homeMember.ReceivesNotifications = true;
        Assert.IsTrue(homeMember.ReceivesNotifications);
    }
    
    [TestMethod]
    public void TestAddPermissionToGetListOfDevicesToMemberRole()
    {
        HomeMember homeMember = new HomeMember();
        homeMember.HasPermissionToListDevices = true;
        Assert.IsTrue(homeMember.HasPermissionToListDevices);
    }
    
    [TestMethod]
    public void TestAddNotificationsToMemberRole()
    {
        HomeMember homeMember = new HomeMember();
        Notification notification = new Notification("Notification");
        homeMember.AddNotification(notification);
        Assert.IsTrue(homeMember.Notifications.Contains(notification));
    }
}