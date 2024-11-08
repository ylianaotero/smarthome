using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;

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
    private const string Alias = "Casa de Juan";
    private const string RoomName = "Living room";
    private const int Id = 11;
    
    private User _user; 
    private User _defaultOwner;

    private Member _member; 

    private DeviceUnitService _deviceUnitService;
    private Room _room;

    private Home _home;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _user = new User();
        _defaultOwner = new User()
        {
            Email = Email1,
            Id = 1,
            Roles = new List<Role>()
            {
                new HomeOwner(),
            }
        };
        
        _home = new Home()
        {
            Owner = _defaultOwner,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
        
        _deviceUnitService = new DeviceUnitService()
        {
            HardwareId = Guid.NewGuid(),
            IsConnected = true,
            Device = new SecurityCamera()
        };

        _room = new Room()
        {
            Id = Id,
            Name = RoomName
        };
        
        _user.Email = Email1;
        _member = new Member(_user); 
    }
    
    [TestCleanup]
    public void TestCleanup()
    {
        _home = null;
        _member = null;
        _deviceUnitService = null;
    }
    
    [TestMethod]
    public void CreateNewHome()
    {
        Home newHome = new Home()
        {
            Owner = _defaultOwner,
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
    public void TestAddAliasToHome()
    {
        _home.Alias = Alias;
        Assert.AreEqual(Alias, _home.Alias);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestAddEmptyAliasToHome()
    {
        _home.Alias = "";
    }
    
    [TestMethod]
    public void TestAddMember()
    {
        _home.AddMember(_member); 
        Assert.AreEqual(1, _home.Members.Count);
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
        
        Member memberWithEmail1 = _home.Members.Find(m => m.User.Email == Email1);
        
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
        _home.AddDevice(_deviceUnitService); 
        
        Assert.AreEqual(1, _home.Devices.Count);
        
        DeviceUnitService device = _home.Devices.Find(d => d.HardwareId == _deviceUnitService.HardwareId);
        
        Assert.IsNotNull(device);
    }
    
        
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestTryToAddExistingDevice()
    {
        _home.AddDevice(_deviceUnitService); 
        
        _home.AddDevice(_deviceUnitService); 
    }
    
        
    [TestMethod]
    public void TestFindDevice()
    {
        _home.AddDevice(_deviceUnitService);

        DeviceUnitService result = _home.FindDevice(_deviceUnitService.HardwareId); 
        
        Assert.AreEqual(_deviceUnitService.HardwareId, result.HardwareId);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TestCannotFindDevice()
    {
        _home.FindDevice(_deviceUnitService.HardwareId); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TestTryToDeleteDevice()
    {
        _home.DeleteDevice(_deviceUnitService.HardwareId); 
    }
    
    [TestMethod]
    public void TestDeleteDevice()
    {
        _home.AddDevice(_deviceUnitService);
        
        _home.DeleteDevice(_deviceUnitService.HardwareId); 
        
        Assert.AreEqual(0, _home.Devices.Count);
    }

    [TestMethod]
    public void TestAddRoomToHome()
    {
       _home.AddRoom(_room);

        Assert.AreEqual(_room.Name, _home.Rooms.FirstOrDefault().Name);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestAddRoomWithTheSameNameToHome()
    {
        _home.AddRoom(_room);

        Room newRoom = new Room()
        {
            Name = _room.Name
        };
        
        _home.AddRoom(newRoom);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestAddRoomWithTheSameIdToHome()
    {
        _home.AddRoom(_room);

        Room newRoom = new Room()
        {
            Id = _room.Id,
            Name = _room.Name + " 2"
        };
        
        _home.AddRoom(newRoom);
    }
}