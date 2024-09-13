using Domain;

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
}