using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using Moq;

namespace TestDomain;

[TestClass]
public class UserTest
{
    private User _user;
    private User _user2;
    private User _updatedUser;
    private Role _administratorRole;
    private Mock<IUserValidator> _userValidatorMock;

    
    private const int ValidId = 1234;
    private const string InvalidName = " ";
    private const string ValidName = "Juan";
    private const string ValidSurname = "Lopez";
    private const string ValidSurname2 = "Perez";
    private const string InvalidEmail = "juanperez";
    private const string ValidPassword = "juanLop1@";
    private const string InvalidPassword = "juanPerez";
    private const string ValidEmail = "juanlopez@gmail.com";
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    
    [TestInitialize]
    public void TestInitialize()
    {
        _user = new User();
        _user2 = new User()
        {
            Name = ValidName,
            Surname = ValidSurname,
            Email = ValidEmail,
            Password = ValidPassword
        };
        _updatedUser = new User()
        {
            Name = ValidName,
            Surname = ValidSurname2,
            Email = ValidEmail,
            Password = ValidPassword
        };
        _administratorRole = new Administrator();
        _userValidatorMock = new Mock<IUserValidator>();
    }


    [TestMethod]
    public void TestAddNameToUser()
    {
        _user.Name = ValidName;
        
        Assert.AreEqual(ValidName, _user.Name);
    }
    
    [TestMethod]
    public void TestAddSurnameToUser()
    {
        _user.Surname = ValidSurname;
        
        Assert.AreEqual(ValidSurname, _user.Surname);
    }
    
    [TestMethod]
    public void TestAddPhotoToUser()
    {
        _user.Photo = ProfilePictureUrl;
        
        Assert.AreEqual(ProfilePictureUrl, _user.Photo);
    }
    
    [TestMethod]
    public void TestAddEmailToUser()
    {
        _user.Email = ValidEmail;

        Assert.AreEqual(ValidEmail, _user.Email);
    }
    
    [TestMethod]
    public void TestAddPasswordToUser()
    {
        _user.Password = ValidPassword;
        
        Assert.AreEqual(ValidPassword, _user.Password);
    }
    
    [TestMethod]
    public void TestAddIdToUser()
    {
        _user.Id = ValidId;
        
        Assert.AreEqual(ValidId, _user.Id);
    }
    
    [TestMethod]
    public void TestCreatedAtUser()
    {
        Assert.AreEqual(DateTime.Now.Date, _user.CreatedAt.Date);
    }
    
    [TestMethod]
    public void TestAddNewRoleToUser()
    {
        Assert.AreEqual(0, _user.Roles.Count());
    }
    
    [TestMethod]
    public void TestAddRoleToUser()
    {
        _user.AddRole(_administratorRole);
        Assert.AreEqual(1, _user.Roles.Count());
    }
    
    [TestMethod]
    public void TestDeleteRoleToUser()
    {
        _user.AddRole(_administratorRole);
        _user.DeleteRole(_administratorRole);
        Assert.AreEqual(0, _user.Roles.Count());
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotDeleteRoleToUser()
    {
        _user.DeleteRole(_administratorRole);
    }
    
    [TestMethod]
    public void TestValidName()
    {
        _userValidatorMock
            .Setup(v => v.ValidateName(ValidName)).Returns(true);
        
        User user = new User(_userValidatorMock.Object);

        user.Name = ValidName; 
        
        _userValidatorMock.VerifyAll();
        
        Assert.AreEqual(ValidName, user.Name);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidName()
    {
        _userValidatorMock
            .Setup(v => v.ValidateName(InvalidName)).Returns(false);
        
        User user = new User(_userValidatorMock.Object);

        user.Name = InvalidName; 
        
        _userValidatorMock.VerifyAll();
    }
    
    [TestMethod]
    public void TestValidSurname()
    {
        _userValidatorMock
            .Setup(v => v.ValidateSurname(ValidSurname)).Returns(true);
        
        User user = new User(_userValidatorMock.Object);

        user.Surname = ValidSurname; 
        
        _userValidatorMock.VerifyAll();
        
        Assert.AreEqual(ValidSurname, user.Surname);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidSurName()
    {
        _userValidatorMock
            .Setup(v => v.ValidateSurname(InvalidName)).Returns(false);
        
        User user = new User(_userValidatorMock.Object);

        user.Surname = InvalidName; 
        
        _userValidatorMock.VerifyAll();
    }
    
    [TestMethod]
    public void TestValidEmail()
    {
        _userValidatorMock
            .Setup(v => v.ValidateEmail(ValidEmail)).Returns(true);
        
        User user = new User(_userValidatorMock.Object);

        user.Email = ValidEmail; 
        
        _userValidatorMock.VerifyAll();
        
        Assert.AreEqual(ValidEmail, user.Email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidEmail()
    {
        _userValidatorMock
            .Setup(v => v.ValidateEmail(InvalidEmail)).Returns(false);
        
        var user = new User(_userValidatorMock.Object);

        user.Email = InvalidEmail; 
        
        _userValidatorMock.VerifyAll();
    }
    
    [TestMethod]
    public void TestValidPassword()
    {
        _userValidatorMock
            .Setup(v => v.ValidatePassword(ValidPassword)).Returns(true);
        
        var user = new User(_userValidatorMock.Object);

        user.Password = ValidPassword; 
        
        _userValidatorMock.VerifyAll();
        
        Assert.AreEqual(ValidPassword, user.Password);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestInvalidPassword()
    {
        _userValidatorMock
            .Setup(v => v.ValidatePassword(InvalidPassword)).Returns(false);
        
        var user = new User(_userValidatorMock.Object);

        user.Password = InvalidPassword; 
        
        _userValidatorMock.VerifyAll();
        
        Assert.AreEqual(InvalidPassword, user.Password);
    }

    [TestMethod]
    public void TestUpdateUser()
    {
        _user2.Update(_updatedUser);
        Assert.AreEqual(_user2.Email,_updatedUser.Email);
    }
}
