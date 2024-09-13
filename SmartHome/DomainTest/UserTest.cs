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
}