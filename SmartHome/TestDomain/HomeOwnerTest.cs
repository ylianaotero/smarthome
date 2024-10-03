using CustomExceptions;
using Domain;

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
        HomeDTO homeDTO = new HomeDTO()
        {
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home(homeDTO);
        homeOwner.AddHome(home);
        Assert.AreEqual(1, homeOwner.Homes.Count());
    }
    
    [TestMethod]
    public void TestDeleteHomeFromHomeOwner()
    {
        HomeDTO homeDTO = new HomeDTO()
        {
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home(homeDTO);
        homeOwner.AddHome(home);
        homeOwner.RemoveHome(home);
        Assert.AreEqual(0, homeOwner.Homes.Count());
    }

    [TestMethod]
    public void TestSearchHomeFromHomeOwner()
    {
        HomeDTO homeDTO = new HomeDTO()
        {
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home(homeDTO);
        homeOwner.AddHome(home);
        Assert.AreEqual(home,homeOwner.SearchHome(home.Id));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotFindHomeFromHomeOwner()
    {
        HomeDTO homeDTO = new HomeDTO()
        {
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home(homeDTO);
        homeOwner.SearchHome(home.Id);
    }
}