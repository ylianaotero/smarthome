using CustomExceptions;
using Domain;

namespace TestDomain;

[TestClass]
public class HomeTest
{
    private const string Street = "Calle 3";
    private const int DoorNumber = 0;
    private const int Latitude = 23;
    private const int Longitude = 34;
    private const string Email1 = "juanperez@gmail.com"; 
    private const string Email2 = "laurasanchez@gmail.com";
    private const int Id = 11;
    private const int Id2 = 000;
    private const int IdToDelete = 11;
    
    private long _homeOwnerId;
    
    private User _user; 

    private Member _member; 

    private Device _device; 

    private Home _home;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _user = new User();
        _home = new Home()
        {
            OwnerId = _homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
        _user.Email = Email1;
        _member = new Member(_user); 
        _device = new SecurityCamera();
        _device.Id = Id;
        _homeOwnerId = Id2;
    }
    
    [TestMethod]
    public void CreateNewHome()
    {
        Home newHome = new Home()
        {
            OwnerId = _homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
        };
        newHome.Id = Id; 
        
        Assert.IsTrue(
            Street == newHome.Street &&
            DoorNumber == newHome.DoorNumber &&
            Latitude == newHome.Latitude &&
            Longitude == newHome.Longitude &&
            Id == newHome.Id &&
            newHome.Members.Count == 0 &&
            newHome.Devices.Count == 0
        );

    }
    
    [TestMethod]
    public void TestAddMember()
    {
        _home.AddMember(_member); 
        Assert.AreEqual(1, _home.Members.Count());
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestTryToAddMemberThatAlreadyExists()
    {
        _home.AddMember(_member); 
        _home.AddMember(_member); 
    }
    
    [TestMethod]
    public void TestFindMember()
    {
        User user2 = new User() { Email = Email2 }; 
        Member member2 = new Member(user2); 
        
        _home.AddMember(_member); 
        _home.AddMember(member2); 
        
        Member result = _home.FindMember(Email1);
        
        Assert.AreEqual(result, _member);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TestCannotFindMember()
    {
        _home.AddMember(_member); 
        
        _home.FindMember(Email2); 
    }
    
    [TestMethod]
    public void TestCheckIfMemberCanReceiveNotificationsDefault()
    {
        
        _home.AddMember(_member); 
        
        bool result = _home.MemberCanReceiveNotifications(_member.User.Email); 
        
        Assert.IsFalse(result);
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
        User user2 = new User() { Email = Email2 }; 
        Member member2 = new Member(user2); 

        _home.AddMember(_member); 
        _home.AddMember(member2); 
        
        _home.DeleteMember(Email1); 
        
        Member memberWithEmail1 = _home.Members.FirstOrDefault(m => m.User.Email == Email1);
        
        Assert.IsNull(memberWithEmail1);
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
        
        Device device = _home.Devices.FirstOrDefault(d => d.Id == Id);
        
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
        
        Assert.AreEqual(Id, result.Id);
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
        
        _home.DeleteDevice(IdToDelete); 
        
        Assert.AreEqual(0, _home.Devices.Count());
        
    }
    
}