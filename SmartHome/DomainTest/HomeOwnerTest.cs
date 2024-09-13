using Domain;
using Domain.DomainExceptions;

namespace DomainTest;

[TestClass]
public class HomeOwnerTest
{
    [TestMethod]
    public void TestAddNewHomeOwner()
    {
        // Arrange
        HomeOwner homeOwner = new HomeOwner();

        // Assert
        Assert.AreEqual(0, homeOwner.Homes.Count());
    }

    [TestMethod]
    public void TestAddNewHomeToHomeOwner()
    {
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home("malibu", 1223, 1333, 1333);
        homeOwner.AddHome(home);
        Assert.AreEqual(1, homeOwner.Homes.Count());
    }
    
    [TestMethod]
    public void TestDeleteHomeFromHomeOwner()
    {
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home("malibu", 1223, 1333, 1333);
        homeOwner.AddHome(home);
        homeOwner.RemoveHome(home);
        Assert.AreEqual(0, homeOwner.Homes.Count());
    }

    [TestMethod]
    public void TestSearchHomeFromHomeOwner()
    {
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home("malibu", 1223, 1333, 1333);
        homeOwner.AddHome(home);
        Assert.AreEqual(home,homeOwner.SearchHome(home.Id));
    }
    
    [TestMethod]
    [ExpectedException(typeof(HomeNotFoundException))]
    public void TestCannotFindHomeFromHomeOwner()
    {
        HomeOwner homeOwner = new HomeOwner();
        Home home = new Home("malibu", 1223, 1333, 1333);
        homeOwner.SearchHome(home.Id);
    }
}