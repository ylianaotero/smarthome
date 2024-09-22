using Domain;
using Domain.Exceptions.GeneralExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.Out;

namespace TestWebApi.Controllers;

[TestClass]
public class UserControllerTest
{
    private const string ErrorMessageWhenCannotFindUser =  "Cannot find user";
    
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
    private User _user_1_example; 
    private User _user_2_example;
    private List<User> _listOfUsers;

    private string _fullName; 
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);

        _userController = new UserController(_userServiceMock.Object,_sessionServiceMock.Object);

        _listOfRoles = new List<Role>();

        _session = new Session(); 
        
        _user_1_example = new User
        {
            Name = Name,
            Email = Email1,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl,
            Roles = _listOfRoles
        };
        
        _user_2_example = new User
        {
            Name = Name,
            Email = Email2,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl,
            Roles = _listOfRoles
        };
        
        _session.User = _user_1_example;
        _session.Id = new Guid(); 
        
        _listOfUsers = new List<User> { _user_1_example, _user_2_example};
        
        _fullName = Name + " " + Surname; 

    }

    [TestMethod]
    public void GetUsersValidRequest()
    {
        _sessionServiceMock.Setup(service => service.GetUser(_session.Id)).Returns(_user_1_example); 
        _userServiceMock.Setup(service => service.IsAdmin(_session.User.Email)).Returns(true); 
        _userServiceMock.Setup(service => service.GetAllUsers()).Returns(_listOfUsers); 
        

        var result = _userController.GetUsers(_session.Id) as OkObjectResult;
        List<UserResponse> userResponse = result.Value as List<UserResponse>;

        _userServiceMock.Verify();
        _sessionServiceMock.Verify();

        Assert.IsTrue(
            result != null &&
            result.StatusCode == 200 &&
            userResponse != null &&
            userResponse.All(user =>
                user.Name == Name &&
                user.Surname == Surname &&
                user.FullName == _fullName &&
                user.CreatedAt.Date == DateTime.Today.Date &&
                user.Roles.Count == 0
            ));
    }
    
    
    [TestMethod]
    public void GetUsersUnauthorized()
    {
        _sessionServiceMock.Setup(service => service.GetUser(_session.Id)).Returns(_user_1_example); 
        _userServiceMock.Setup(service => service.IsAdmin(_session.User.Email)).Returns(false); 
        
        var result = _userController.GetUsers(_session.Id) as ObjectResult;

        _userServiceMock.Verify();
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(403, result.StatusCode);
    }
    
    [TestMethod]
    public void GetUsersOtherException()
    {
        _sessionServiceMock.Setup(service => service.GetUser(_session.Id)).Throws(new Exception()); 
        _userServiceMock.Setup(service => service.IsAdmin(_session.User.Email)).Returns(false); 
        
        var result = _userController.GetUsers(_session.Id) as ObjectResult;

        _userServiceMock.Verify();
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(500, result.StatusCode);
    }
    
    [TestMethod]
    public void GetUsersInvalidToken()
    {
        _sessionServiceMock.Setup(service => service.GetUser(_session.Id)).Throws(new CannotFindItemInList(ErrorMessageWhenCannotFindUser )); 
        _userServiceMock.Setup(service => service.IsAdmin(_session.User.Email)).Returns(true); 
        
        var result = _userController.GetUsers(_session.Id) as ObjectResult;

        _userServiceMock.Verify();
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(401, result.StatusCode);
    }
    
    
}