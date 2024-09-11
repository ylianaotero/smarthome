using Domain;

namespace DomainTest;

[TestClass]
public class HomeOwnerTest
{
    [TestMethod]
    public void TestAddNameToHomeOwner()
    {
        // Arrange
        HomeOwner homeOwner = new HomeOwner();
        
        // Act
        homeOwner.Name = "Pedro";

        // Assert
        Assert.AreEqual("Pedro", homeOwner.Name);
    }
    
    [TestMethod]
    public void TestAddSurnameToHomeOwner()
    {
        // Arrange
        HomeOwner homeOwner = new HomeOwner();
        
        // Act
        homeOwner.Surname = "Gonzalez";

        // Assert
        Assert.AreEqual("Gonzalez", homeOwner.Surname);
    }
}