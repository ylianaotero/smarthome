using Domain;

namespace DomainTest;

[TestClass]
public class AdministratorTest
{
    [TestMethod]
    public void TestAddNameToAdministrator()
    {
        // Arrange
        Administrator administrator = new Administrator();
        
        // Act
        administrator.Name = "Juan";

        // Assert
        Assert.AreEqual("Juan", administrator.Name);
    }
    
    [TestMethod]
    public void TestAddSurnameToAdministrator()
    {
        // Arrange
        Administrator administrator = new Administrator();
        
        // Act
        administrator.Surname = "Perez";

        // Assert
        Assert.AreEqual("Perez", administrator.Surname);
    }
    
    [TestMethod]
    public void TestAddEmailToAdministrator()
    {
        // Arrange
        Administrator administrator = new Administrator();
        
        // Act
        administrator.Email = "juanperez@gmail.com";

        // Assert
        Assert.AreEqual("juanperez@gmail.com", administrator.Email);
    }

}