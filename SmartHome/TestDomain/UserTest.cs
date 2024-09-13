using Domain;
using Domain.Exceptions.RoleExceptions;

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
    
    [TestMethod]
    public void TestAddSurnameToUser()
    {
        // Arrange
        User user = new User();
        
        // Act
        user.Surname = "Lopez";

        // Assert
        Assert.AreEqual("Lopez", user.Surname);
    }
    
    [TestMethod]
    public void TestAddEmailToUser()
    {
        // Arrange
        User user = new User();
        
        // Act
        user.Email = "juanlopez@gmail.com";

        // Assert
        Assert.AreEqual("juanlopez@gmail.com", user.Email);
    }
    
    [TestMethod]
    public void TestAddPasswordToUser()
    {
        // Arrange
        User user = new User();
        
        // Act
        user.Password = "juanlop1";

        // Assert
        Assert.AreEqual("juanlop1", user.Password);
    }
    
    [TestMethod]
    public void TestAddNewRoleToUser()
    {
        User user = new User();
        
        Assert.AreEqual(0, user.Roles.Count());
    }
    
    [TestMethod]
    public void TestAddRoleToUser()
    {
        User user = new User();
        Role role = new Administrator();
        user.AddRole(role);
        Assert.AreEqual(1, user.Roles.Count());
    }
    
    [TestMethod]
    public void TestDeleteRoleToUser()
    {
        User user = new User();
        Role role = new Administrator();
        user.AddRole(role);
        user.DeleteRole(role);
        Assert.AreEqual(0, user.Roles.Count());
    }
    
    [TestMethod]
    [ExpectedException(typeof(RoleNotFoundException))]
    public void TestCannotDeleteRoleToUser()
    {
        User user = new User();
        Role role = new Administrator();
        user.DeleteRole(role);
    }
}