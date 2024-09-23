using Domain;
using Domain.Exceptions.GeneralExceptions;

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
    
    private long _homeOwnerId;

    private Member _member; 

    private Device _device; 

    private Home _home; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _home = new Home(_homeOwnerId,Street,DoorNumber,Latitude,Longitude);
        _member = new Member();
        _member.Email = Email1;
        _member.Permission = false; 
        _device = new SecurityCamera();
        _device.Id = Id;
        _homeOwnerId = 000;
    }
    
    [TestMethod]
    public void CreateNewHome()
    {
        Home newHome = new Home(_homeOwnerId,Street,DoorNumber,Latitude,Longitude);
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
        Member member = new Member();
        _home.AddMember(member); 
        Assert.AreEqual(1, _home.Members.Count());
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestTryToAddMemberThatAlreadyExists()
    {
        Member member2 = new Member(); 
        member2.Email = Email2;
        member2.Permission = true; 
        
        _home.AddMember(_member); 
        _home.AddMember(member2); 
        
        _home.AddMember(_member); 
    }
    
    [TestMethod]
    public void TestFindMember()
    {
        Member member2 = new Member(); 
        member2.Email = Email2;
        member2.Permission = true; 
        
        _home.AddMember(_member); 
        _home.AddMember(member2); 
        
        Member result = _home.FindMember(Email2); 
        
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
        Member member2 = new Member(); 
        member2.Email = Email2;
        member2.Permission = true; 
        
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
        Member member2 = new Member(); 
        member2.Email = Email2;
        member2.Permission = true; 
        
        _home.AddMember(_member); 
        _home.AddMember(member2); 
        
        _home.DeleteMember(Email1); 
        
        Assert.AreEqual(1, _home.Members.Count);
        
        var memberWithEmail1 = _home.Members.FirstOrDefault(m => m.Email == Email1);
        
        Assert.IsNull( memberWithEmail1);

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
        
        _home.DeleteDevice(11); 
        
        Assert.AreEqual(0, _home.Devices.Count());
        
    }
    
}