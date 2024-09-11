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
    
    [TestMethod]
    public void TestAddEmailToHomeOwner()
    {
        // Arrange
        HomeOwner homeOwner = new HomeOwner();
        
        // Act
        homeOwner.Email = "pedroGonzalez@gmail.com";

        // Assert
        Assert.AreEqual("pedroGonzalez@gmail.com", homeOwner.Email);
    }
    
    [TestMethod]
    public void TestAddPasswordToHomeOwner()
    {
        // Arrange
        HomeOwner homeOwner = new HomeOwner();
        
        // Act
        homeOwner.Password = "password1";

        // Assert
        Assert.AreEqual("password1", homeOwner.Password);
    }
}