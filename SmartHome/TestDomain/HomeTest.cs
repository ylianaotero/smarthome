using Domain;
using Domain.Exceptions.GeneralExceptions;
using IDomain;

namespace DomainTest;

[TestClass]
public class HomeTest
{
    private const string Street = "Calle 3";
    private const int DoorNumber = 0;
    private const int Latitude = 23;
    private const int Longitude = 34;
    private const string Email1 = "juanperez@gmail.com"; 
    private const string Email2 = "laurasanchez@gmail.com";
    private const long id = 11;

    private (User, bool) _member; 
    
    private User _user;

    private Device _device; 

    private Home _home; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _home = new Home(Street,DoorNumber,Latitude,Longitude);
        _user = new User();
        _user.Email = Email1;
        _member = (_user, false);
        _device = new SecurityCamera();
        _device.Id = id; 
    }
    
    [TestMethod]
    public void CreateNewHome()
    {
        Home newHome = new Home(Street,DoorNumber,Latitude,Longitude);
        newHome.Id = 1; 
        
        Assert.AreEqual(Street, newHome.Street);
        Assert.AreEqual(DoorNumber, newHome.DoorNumber);
        Assert.AreEqual(Latitude, newHome.Latitude);
        Assert.AreEqual(Longitude, newHome.Longitude);
        Assert.AreEqual(1, newHome.Id);
        Assert.AreEqual(0, newHome.Members.Count);
        Assert.AreEqual(0, newHome.Devices.Count);
    }
    
    [TestMethod]
    public void TestAddMember()
    {
        User user = new User();
        (User, bool) member = (user, true);
        _home.AddMember(member); 
        Assert.AreEqual(1, _home.Members.Count());
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestTryToAddMemberThatAlreadyExists()
    {
        User user = new User();
        user.Email = Email2; 
        (User, bool) member2 = (user, true);
        
        _home.AddMember(_member); 
        _home.AddMember(member2); 
        
        (User, bool) existingMember = (user, false);
        
        _home.AddMember(existingMember); 
    }
    
    [TestMethod]
    public void TestFindMember()
    {
        User user = new User();
        user.Email = Email2; 
        (User, bool) member2 = (user, true);
        
        _home.AddMember(_member); 
        _home.AddMember(member2); 
        
        (User, bool) result = _home.FindMember(Email2); 
        
        Assert.AreEqual(member2, result);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TestCannotFindMember()
    {
        _home.AddMember(_member); 
        
        _home.FindMember(Email2); 
    }
    
    [TestMethod]
    public void TestCheckIfMemberCanReceiveNotifications()
    {
        User user = new User();
        user.Email = Email2; 
        (User, bool) member2 = (user, true);
        
        _home.AddMember(_member); 
        _home.AddMember(member2); 
        
        bool result = _home.MemberCanReceiveNotifications(Email2); 
        
        Assert.IsTrue(result);
    }
    
        
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TestCheckIfMemberThatDoesNotExistCanReceiveNotifications()
    {
        _home.AddMember(_member); 
        
        _home.MemberCanReceiveNotifications(Email2); 
    }
    
    [TestMethod]
    public void TestDeleteMember()
    {
        User user = new User();
        user.Email = Email2; 
        (User, bool) member2 = (user, true);
        
        _home.AddMember(_member); 
        _home.AddMember(member2); 
        
        _home.DeleteMember(Email1); 
        
        Assert.AreEqual(1, _home.Members.Count);
        
        var memberWithEmail1 = _home.Members.FirstOrDefault(m => m.Item1.Email == Email1);
        Assert.IsNull( memberWithEmail1.Item1);

    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TestDeleteMemberThatDoesNotExist()
    {
        _home.AddMember(_member); 
        
        _home.DeleteMember(Email2); 

    }
    
    [TestMethod]
    public void TestAddDevice()
    {
        _home.AddDevice(_device); 
        
        Assert.AreEqual(1, _home.Devices.Count());
        
        Device device = _home.Devices.FirstOrDefault(d => d.Id == id);
        Assert.IsNotNull(device);
    }
    
        
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestTryToAddExistingDevice()
    {
        _home.AddDevice(_device); 
        
        _home.AddDevice(_device); 
    }
    
        
    [TestMethod]
    public void TestFindDevice()
    {
        _home.AddDevice(_device);

        Device result = _home.FindDevice(_device.Id); 
        
        Assert.AreEqual(id, result.Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TestCannotFindDevice()
    {
        _home.FindDevice(_device.Id); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TestTryToDeleteDevice()
    {
        _home.DeleteDevice(_device.Id); 
    }
    
    [TestMethod]
    public void TestDeleteDevice()
    {
        _home.AddDevice(_device);
        
        _home.DeleteDevice(11); 
        
        Assert.AreEqual(0, _home.Devices.Count());
        
    }

 

    
}