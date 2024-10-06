using CustomExceptions;
using Domain;
using Domain.Abstract;
using Domain.Concrete;
using Moq;

namespace TestDomain;

[TestClass]
public class UserTest
{
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private const string ValidName = "Juan";
    private const string ValidSurname = "Lopez";
    private const string ValidSurname2 = "Perez";
    private const string ValidEmail = "juanlopez@gmail.com";
    private const string ValidPassword = "juanLop1@";
    private const int ValidId = 1234;
    private const string InvalidName = " ";
    private const string InvalidEmail = "juanperez";
    private const string InvalidPassword = "juanPerez";

    [TestMethod]
    public void TestAddNameToUser()
    {
        User user = new User();
        
        user.Name = ValidName;
        
        Assert.AreEqual(ValidName, user.Name);
    }
    
    [TestMethod]
    public void TestAddSurnameToUser()
    {
        User user = new User();
        
        user.Surname = ValidSurname;
        
        Assert.AreEqual(ValidSurname, user.Surname);
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
        
        user.Email = ValidEmail;

        Assert.AreEqual(ValidEmail, user.Email);
    }
    
    [TestMethod]
    public void TestAddPasswordToUser()
    {
        User user = new User();
        
        user.Password = ValidPassword;
        
        Assert.AreEqual(ValidPassword, user.Password);
    }
    
    [TestMethod]
    public void TestAddIdToUser()
    {
        User user = new User();
        
        user.Id = ValidId;
        
        Assert.AreEqual(ValidId, user.Id);
    }
    
    [TestMethod]
    public void TestCreatedAtUser()
    {
        User user = new User();
        
        Assert.AreEqual(DateTime.Now.Date, user.CreatedAt.Date);
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
    [ExpectedException(typeof(ElementNotFound))]
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
            .Setup(v => v.ValidateName(ValidName)).Returns(true);
        
        var user = new User(userValidatorMock.Object);

        user.Name = ValidName; 
        
        userValidatorMock.VerifyAll();
        
        Assert.AreEqual(ValidName, user.Name);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidName()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateName(InvalidName)).Returns(false);
        
        var user = new User(userValidatorMock.Object);

        user.Name = InvalidName; 
        
        userValidatorMock.VerifyAll();
    }
    
    [TestMethod]
    public void TestValidSurname()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateSurname(ValidSurname)).Returns(true);
        
        var user = new User(userValidatorMock.Object);

        user.Surname = ValidSurname; 
        
        userValidatorMock.VerifyAll();
        
        Assert.AreEqual(ValidSurname, user.Surname);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidSurName()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateSurname(InvalidName)).Returns(false);
        
        var user = new User(userValidatorMock.Object);

        user.Surname = InvalidName; 
        
        userValidatorMock.VerifyAll();
    }
    
    [TestMethod]
    public void TestValidEmail()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateEmail(ValidEmail)).Returns(true);
        
        var user = new User(userValidatorMock.Object);

        user.Email = ValidEmail; 
        
        userValidatorMock.VerifyAll();
        
        Assert.AreEqual(ValidEmail, user.Email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidEmail()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidateEmail(InvalidEmail)).Returns(false);
        
        var user = new User(userValidatorMock.Object);

        user.Email = InvalidEmail; 
        
        userValidatorMock.VerifyAll();
    }
    
    [TestMethod]
    public void TestValidPassword()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidatePassword(ValidPassword)).Returns(true);
        
        var user = new User(userValidatorMock.Object);

        user.Password = ValidPassword; 
        
        userValidatorMock.VerifyAll();
        
        Assert.AreEqual(ValidPassword, user.Password);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidPassword()
    {
        var userValidatorMock = new Mock<IUserValidator>();
        
        userValidatorMock
            .Setup(v => v.ValidatePassword(InvalidPassword)).Returns(false);
        
        var user = new User(userValidatorMock.Object);

        user.Password = InvalidPassword; 
        
        userValidatorMock.VerifyAll();
        
        Assert.AreEqual(InvalidPassword, user.Password);
    }

    [TestMethod]
    public void TestUpdateUser()
    {
        User user = new User()
        {
            Name = ValidName,
            Surname = ValidSurname,
            Email = ValidEmail,
            Password = ValidPassword
        };
        User userUpdated = new User();
        userUpdated.Name = ValidName;
        userUpdated.Surname = ValidSurname2;
        userUpdated.Email = ValidEmail;
        userUpdated.Password = ValidPassword;
        user.Update(userUpdated);
        Assert.AreEqual(user.Email,userUpdated.Email);
    }
}
