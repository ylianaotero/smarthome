using Domain;
using IBusinessLogic;
using IDataAccess;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using Moq;
using WebApi.Controllers;

namespace TestWebApi.Controllers;

[TestClass]
public class UserControllerTest
{
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private const string Name =  "John";
    private const string Email1 = "john.doe@example.com";
    private const string Email2 = "john.lopez@example.com";
    private const string Password = "Securepassword1@";
    private const string Surname = "Doe";
    private const int Page = 1;
    private const int PageSize = 10;
    private const string Role = "Administrator";

    private List<Role> _listOfRoles;
    private Session _session; 
    
    private Mock<IUserService> _userServiceMock;
    private Mock<ISessionService> _sessionServiceMock;
    private UserController _userController;
    private User _user_1_example; 
    private User _user_2_example;
    private User _user_3_example;
    private List<User> _listOfUsers;

    private string _fullName; 
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);

        _userController = new UserController(_userServiceMock.Object,_sessionServiceMock.Object);
        Role role = new Administrator();
        _listOfRoles = new List<Role>() {};

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
        
        _user_3_example = new User
        {
            Name = Name,
            Email = Email2,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl,
            Roles = new List<Role>(){role}
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
        _userServiceMock
            .Setup(service => service.GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers); 
        UsersRequest request = new UsersRequest();
        UsersResponse expectedResponse = new UsersResponse(_listOfUsers);
        
        var result = _userController.GetUsers(request, DefaultPageDataRequest()) as OkObjectResult;
        UsersResponse userResponse = result.Value as UsersResponse;

        _userServiceMock.Verify();
        _sessionServiceMock.Verify();

        Assert.AreEqual(expectedResponse, userResponse);
    }
    
    [TestMethod]
    public void GetUsersByFullNameAndRole()
    {
        UsersRequest request = new UsersRequest
        {
            FullName = _fullName,
            Role = Role
        };
        List<User> listOfUsers =
        [
            _user_3_example,
        ];
        _userServiceMock
            .Setup(service => service.GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(listOfUsers); 
        UsersResponse expectedResponse = new UsersResponse(listOfUsers);
        
        var result = _userController.GetUsers(request, DefaultPageDataRequest()) as OkObjectResult;
        UsersResponse usersResponse = result.Value as UsersResponse;

        _userServiceMock.Verify();

        Assert.AreEqual(expectedResponse, usersResponse);
    }
    
    private PageDataRequest DefaultPageDataRequest()
    {
        PageDataRequest request = new PageDataRequest();
        
        request.Page = Page;
        request.PageSize = PageSize;
        
        return request;
    }
}