using Domain;

namespace DomainTest;

[TestClass]
public class UserTest
{
    [TestMethod]
    public void TestAddNameToUser()
    {
        // Arrange
        User user = new User();
        
        // Act
        user.Name = "Juan";

        // Assert
        Assert.AreEqual("Juan", user.Name);
    }
    
    public void TestAddSurnameToUser()
    {
        // Arrange
        User user = new User();
        
        // Act
        user.Surname = "Lopez";

        // Assert
        Assert.AreEqual("Lopez", user.Surname);
    }
}