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

    private (User, bool) _member; 
    
    private User _user; 

    private Home _home; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _home = new Home(Street,DoorNumber,Latitude,Longitude);
        _user = new User();
        _user.Email = Email1;
        _member = (_user, false); 

    }
    
    [TestMethod]
    public void CreateNewHome()
    {
        // Arrange
        Home newHome = new Home(Street,DoorNumber,Latitude,Longitude);

        // Assert
        Assert.AreEqual(Street, newHome.Street);
        Assert.AreEqual(DoorNumber, newHome.DoorNumber);
        Assert.AreEqual(Latitude, newHome.Latitude);
        Assert.AreEqual(Longitude, newHome.Longitude);
        Assert.AreEqual(0, newHome.Members.Count);
        Assert.AreEqual(0, newHome.Devices.Count);
    }
    
    [TestMethod]
    public void TestAddUserAndBoolToMembers()
    {
        User user = new User();
        (User, bool) input = (user, true);
        _home.AddMember(input); 
        Assert.AreEqual(1, _home.Members.Count());
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
    
 

    
}