using Domain;

namespace TestDomain;

[TestClass]
public class HomeTest
{
    private string _street = "Calle 3";
    private int _doorNumber = 0;
    private int _latitude = 23;
    private int _longitude = 34;

    private Home _home; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _home = new Home(_street,_doorNumber,_latitude,_longitude);
    }
    
    [TestMethod]
    public void CreateNewHome()
    {
        // Arrange
        Home newHome = new Home(_street,_doorNumber,_latitude,_longitude);

        // Assert
        Assert.AreEqual(_street, newHome.Street);
        Assert.AreEqual(_doorNumber, newHome.DoorNumber);
        Assert.AreEqual(_latitude, newHome.Latitude);
        Assert.AreEqual(_longitude, newHome.Longitude);
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
    
 

    
}