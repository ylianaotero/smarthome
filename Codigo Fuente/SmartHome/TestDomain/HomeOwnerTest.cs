using CustomExceptions;
using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class HomeOwnerTest
{
    private Home _home;
    private HomeOwner _homeOwner;
    
    private const int DoorNumber = 1223;
    private const string Street = "malibu";
    private const int Latitude = 1333;
    private const int Longitude = 1333;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _homeOwner = new HomeOwner();
        _home = new Home()
        {
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
    }
    
    [TestMethod]
    public void TestAddNewHomeOwner()
    {
        Assert.AreEqual(0 , _homeOwner.Homes.Count());
    }

    [TestMethod]
    public void TestAddNewHomeToHomeOwner()
    {
        _homeOwner.AddHome(_home);
        Assert.AreEqual(1, _homeOwner.Homes.Count());
    }
    
    [TestMethod]
    public void TestDeleteHomeFromHomeOwner()
    {
        _homeOwner.AddHome(_home);
        _homeOwner.RemoveHome(_home);
        Assert.AreEqual(0, _homeOwner.Homes.Count());
    }

    [TestMethod]
    public void TestSearchHomeFromHomeOwner()
    {
        _homeOwner.AddHome(_home);
        Assert.AreEqual(_home,_homeOwner.SearchHome(_home.Id));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotFindHomeFromHomeOwner()
    {
        _homeOwner.SearchHome(_home.Id);
    }
}