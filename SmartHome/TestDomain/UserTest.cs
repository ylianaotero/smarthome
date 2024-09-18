using Domain;
using Domain.Exceptions.RoleExceptions;
using Domain.Exceptions.GeneralExceptions;
using IDomain;
using Moq;

namespace DomainTest;

[TestClass]
public class UserTest
{
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    
    [TestMethod]
    public void TestAddNameToUser()
    {
        User user = new User();
        
        user.Name = "Juan";
        
        Assert.AreEqual("Juan", user.Name);
    }
    
    [TestMethod]
    public void TestAddSurnameToUser()
    {
        User user = new User();
        
        user.Surname = "Lopez";
        
        Assert.AreEqual("Lopez", user.Surname);
    }
    
    [TestMethod]
    public void TestAddPhotoToUser()
    {
        User user = new User();
        
        user.Photo = ProfilePictureUrl;
        
        Assert.AreEqual(ProfilePictureUrl, user.Photo);
    }
    
    [TestMethod]
    public void TestAddEmailToUser()
    {
        User user = new User();
        
        user.Email = "juanlopez@gmail.com";

        Assert.AreEqual("juanlopez@gmail.com", user.Email);
    }
    
    [TestMethod]
    public void TestAddPasswordToUser()
    {
        User user = new User();
        
        user.Password = "juanLop1@";
        
        Assert.AreEqual("juanLop1@", user.Password);
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
    
    [TestMethod]
    public void TestValidName()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateName("Juan")).Returns(true);
        
        var user = new User(userValidatorMock.Object);

        user.Name = "Juan"; 
        
        userValidatorMock.Verify(v => v.ValidateName("Juan"), Times.Once, "ValidateName should be called with 'Juan'");
        
        Assert.AreEqual("Juan", user.Name);
        
        userValidatorMock.VerifyAll();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidName()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateName(" ")).Returns(false);
        
        var user = new User(userValidatorMock.Object);

        user.Name = " "; 
        
        userValidatorMock.Verify(v => v.ValidateName(" "), Times.Once, "ValidateName should be called with ' '");
        
        userValidatorMock.VerifyAll();
    }
    
    [TestMethod]
    public void TestValidSurname()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateSurname("Perez")).Returns(true);
        
        var user = new User(userValidatorMock.Object);

        user.Surname = "Perez"; 
        
        userValidatorMock.Verify(v => v.ValidateSurname("Perez"), Times.Once, "ValidateSurName should be called with 'Perez'");
        
        Assert.AreEqual("Perez", user.Surname);
        
        userValidatorMock.VerifyAll();
    }
    
        
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidSurName()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateSurname(" ")).Returns(false);
        
        var user = new User(userValidatorMock.Object);

        user.Surname = " "; 
        
        userValidatorMock.Verify(v => v.ValidateSurname(" "), Times.Once, "ValidateSurname should be called with ' '");
        
        userValidatorMock.VerifyAll();
    }
    
    [TestMethod]
    public void TestValidEmail()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateEmail("juanperez@gmail.com")).Returns(true);
        
        var user = new User(userValidatorMock.Object);

        user.Email = "juanperez@gmail.com"; 
        
        userValidatorMock.Verify(v => v.ValidateEmail("juanperez@gmail.com"), Times.Once, "ValidateEmail should be called with 'juanperez@gmail.com'");
        
        Assert.AreEqual("juanperez@gmail.com", user.Email);
        
        userValidatorMock.VerifyAll();
    }
    
        
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidEmail()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateEmail("juanperez")).Returns(false);
        
        var user = new User(userValidatorMock.Object);

        user.Email = "juanperez"; 
        
        userValidatorMock.Verify(v => v.ValidateEmail("juanperez"), Times.Once, "ValidateEmail should be called with 'juanperez'");
        
        userValidatorMock.VerifyAll();
    }
    
    [TestMethod]
    public void TestValidPassword()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidatePassword("juanPerez@.")).Returns(true);
        
        var user = new User(userValidatorMock.Object);

        user.Password = "juanPerez@."; 
        
        userValidatorMock.Verify(v => v.ValidatePassword("juanPerez@."), Times.Once, "ValidatePassword should be called with 'juanPerez@.'");
        
        Assert.AreEqual("juanPerez@.", user.Password);
        
        userValidatorMock.VerifyAll();
    }
    
        
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidPassword()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidatePassword("juanPerez")).Returns(false);
        
        var user = new User(userValidatorMock.Object);

        user.Password = "juanPerez"; 
        
        userValidatorMock.Verify(v => v.ValidatePassword("juanPerez"), Times.Once, "ValidatePassword should be called with 'juanPerez'");
        
        Assert.AreEqual("juanPerez", user.Password);
        
        userValidatorMock.VerifyAll();
    }
}