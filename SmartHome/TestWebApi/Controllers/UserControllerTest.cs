using BusinessLogic.IServices;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Out;

namespace TestWebApi;

[TestClass]
public class UserControllerTest
{
   private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private const string Name =  "John";
    private const string Email1 = "john.doe@example.com";
    private const string Email2 = "john.lopez@example.com";
    private const string Password = "Securepassword1@";
    private const string Surname = "Doe";

    private List<Role> _listOfRoles;
    private Session _session; 
    
    private Mock<IUserService> _userServiceMock;
    private Mock<ISessionService> _sessionServiceMock;
    private UserController _userController;
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);

        _userController = new UserController(_userServiceMock.Object,_sessionServiceMock.Object);

        _listOfRoles = new List<Role>();

        _session = new Session(); 

    }

    [TestMethod]
    public void CreateUserValidRequest()
    {

        var user1 = new User
        {
            Name = Name,
            Email = Email1,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl,
            Roles = _listOfRoles
        };
        
        var user2 = new User
        {
            Name = Name,
            Email = Email2,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl,
            Roles = _listOfRoles
        };

        _session.User = user1;
        _session.Id = new Guid(); 

        string fullName = Name + " " + Surname; 

        List<User> listOfUsers = new List<User> { user1, user2 };
        _sessionServiceMock.Setup(service => service.GetUser(_session.Id)).Returns(user1); 
        _userServiceMock.Setup(service => service.IsAdmin(_session.User.Email)).Returns(true); 
        _userServiceMock.Setup(service => service.GetAllUsers()).Returns(listOfUsers); 
        

        var result = _userController.GetUsers(_session.Id) as OkObjectResult;
        List<UserResponse> userResponse = result.Value as List<UserResponse>;

        _userServiceMock.Verify(service => service.GetAllUsers(), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsNotNull(userResponse);
        foreach (var user in userResponse)
        {
            Assert.AreEqual(Name, user.Name);
            Assert.AreEqual(Surname, user.Surname);
            Assert.AreEqual( fullName, user.FullName);
            Assert.AreEqual(DateTime.Today.Date, user.CreatedAt.Date);
            Assert.AreEqual(0, user.Roles.Count);
        }
    }
    
    
    [TestMethod]
    public void CreateUserInvalidRequest()
    {

        var user1 = new User
        {
            Name = Name,
            Email = Email1,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl,
            Roles = _listOfRoles
        };

        var user2 = new User(); 

        _session.User = user1;
        _session.Id = new Guid(); 

        string fullName = Name + " " + Surname; 

        List<User> listOfUsers = new List<User> { user1, user2 };
        _sessionServiceMock.Setup(service => service.GetUser(_session.Id)).Returns(user1); 
        _userServiceMock.Setup(service => service.IsAdmin(_session.User.Email)).Returns(false); 
        
        var result = _userController.GetUsers(_session.Id) as ObjectResult;

        _userServiceMock.Verify();
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(403, result.StatusCode);
    }
    
    
}