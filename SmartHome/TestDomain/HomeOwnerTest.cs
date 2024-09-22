using Domain;
using Domain.Exceptions.HomeExceptions;

namespace TestDomain;

[TestClass]
public class HomeOwnerTest
{
    private const int DoorNumber = 1223;
    private const string Street = "malibu";
    private const int Latitude = 1333;
    private const int Longitude = 1333;
    
    [TestMethod]
    public void TestAddNewHomeOwner()
    {
        HomeOwner homeOwner = new HomeOwner();
        
        Assert.AreEqual(0 , homeOwner.Homes.Count());
    }

    [TestMethod]
    public void TestAddNewHomeToHomeOwner()
    {
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home(Street, DoorNumber, Latitude, Longitude);
        homeOwner.AddHome(home);
        Assert.AreEqual(1, homeOwner.Homes.Count());
    }
    
    [TestMethod]
    public void TestDeleteHomeFromHomeOwner()
    {
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home(Street, DoorNumber, Latitude, Longitude);
        homeOwner.AddHome(home);
        homeOwner.RemoveHome(home);
        Assert.AreEqual(0, homeOwner.Homes.Count());
    }

    [TestMethod]
    public void TestSearchHomeFromHomeOwner()
    {
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home(Street, DoorNumber, Latitude, Longitude);
        homeOwner.AddHome(home);
        Assert.AreEqual(home,homeOwner.SearchHome(home.Id));
    }
    
    [TestMethod]
    [ExpectedException(typeof(HomeNotFoundException))]
    public void TestCannotFindHomeFromHomeOwner()
    {
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home(Street, DoorNumber, Latitude, Longitude);
        homeOwner.SearchHome(home.Id);
    }
}