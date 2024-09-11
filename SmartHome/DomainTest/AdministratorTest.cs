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
        administrator.Surname = "Juan";

        // Assert
        Assert.AreEqual("Juan", administrator.Surname);
    }

}