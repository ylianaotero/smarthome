using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using Moq;
using WebApi.Controllers;
using InputNotValid = CustomExceptions.InputNotValid;

namespace TestWebApi.Controllers;

[TestClass]
public class UserControllerTest
{
    private const int Page = 1;
    private const int PageSize = 10;
    private const string Name =  "John";
    private const string Surname = "Doe";
    private const string Email1 = "john.doe@example.com";
    private const string Email2 = "john.lopez@example.com";
    private const string Password = "Securepassword1@";
    private const string CannotFindItemMessage = "Cannot find item in list";
    private const string CannotFindUserMessage = "Cannot find user";
    private const string InputNotValidMessage = "Input not valid";
    private const string CannotAddItemMessage = "Cannot add item";
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private const string AdministratorRole = "Administrator";
    private const string HomeOwnerRole = "HomeOwner";
    
    
    private List<Role> _listOfRoles;
    private Session _session; 
    
    private Mock<IUserService> _userServiceMock;
    private UserController _userController;
    private User _user_1_example; 
    private User _user_2_example;
    private User _user_3_example;
    private List<User> _listOfUsers;
    private string _fullName;

    [TestInitialize]
    public void TestInitialize()
    {
        SetupDefaultMock();
        SetupDefaultUsers();
        SetupDefaultSession();
        
        _fullName = Name + " " + Surname;
    }

    [TestMethod]
    public void GetUsersValidRequest()
    {
        _userServiceMock.Setup(service => service.IsAdmin(_session.User.Email)).Returns(true); 
        _userServiceMock
            .Setup(service => service.GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers); 
        _userController = new UserController(_userServiceMock.Object);
        GetUsersRequest request = new GetUsersRequest();
        GetUsersResponse expectedResponse = new GetUsersResponse(_listOfUsers);
        
        ObjectResult result = _userController.GetUsers(request, DefaultPageDataRequest()) as OkObjectResult;
        GetUsersResponse getUserResponse = result.Value as GetUsersResponse;

        _userServiceMock.Verify();

        Assert.AreEqual(expectedResponse, getUserResponse);
    }
    
    [TestMethod]
    public void GetUsersByFullNameAndRole()
    {
        GetUsersRequest request = new GetUsersRequest
        {
            FullName = _fullName,
            Role = AdministratorRole
        };
        List<User> listOfUsers =
        [
            _user_3_example,
        ];
        _userServiceMock
            .Setup(service => service
                .GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(listOfUsers); 
        _userController = new UserController(_userServiceMock.Object);
        GetUsersResponse expectedResponse = new GetUsersResponse(listOfUsers);
        
        ObjectResult result = _userController.GetUsers(request, DefaultPageDataRequest()) as OkObjectResult;
        GetUsersResponse getUsersResponse = result.Value as GetUsersResponse;

        _userServiceMock.Verify();

        Assert.AreEqual(expectedResponse, getUsersResponse);
    }

    [TestMethod]
    public void CannotGetUsers()
    {
        _userServiceMock
            .Setup(service => service
                .GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Throws(new CannotFindItemInList(CannotFindItemMessage));
        _userController = new UserController(_userServiceMock.Object);
        
        _userController.GetUsers(new GetUsersRequest(), DefaultPageDataRequest());
    }

    [TestMethod]
    public void AddRoleToUserOkStatusCode()
    {
        _userServiceMock
            .Setup(service => service.AssignRoleToUser(It.IsAny<long>(), It.IsAny<string>()));
        _userController = new UserController(_userServiceMock.Object);
        PostUserRoleRequest request = new PostUserRoleRequest
        {
            Role = HomeOwnerRole,
        };
        
        ObjectResult result = _userController.PostUserRole(_user_1_example.Id, request) as OkObjectResult;

        _userServiceMock.Verify();

        Assert.AreEqual(200, result.StatusCode);
    }
    
    [TestMethod]
    public void AddRoleToUserNotFoundStatusCode()
    {
        _userServiceMock
            .Setup(service => service.AssignRoleToUser(It.IsAny<long>(), It.IsAny<string>()))
            .Throws(new ElementNotFound(CannotFindUserMessage));
        _userController = new UserController(_userServiceMock.Object);
        PostUserRoleRequest request = new PostUserRoleRequest
        {
            Role = HomeOwnerRole,
        };
        
        ObjectResult result = _userController.PostUserRole(_user_1_example.Id, request) as NotFoundObjectResult;

        _userServiceMock.Verify();

        Assert.AreEqual(404, result.StatusCode);
    }
    
    [TestMethod]
    public void AddRoleToUserBadRequestStatusCode()
    {
        _userServiceMock
            .Setup(service => service.AssignRoleToUser(It.IsAny<long>(), It.IsAny<string>()))
            .Throws(new InputNotValid(InputNotValidMessage));
        _userController = new UserController(_userServiceMock.Object);
        PostUserRoleRequest request = new PostUserRoleRequest
        {
            Role = Random.Shared.Next().ToString()
        };
        
        ObjectResult result = _userController.PostUserRole(_user_1_example.Id, request) as BadRequestObjectResult;

        _userServiceMock.Verify();

        Assert.AreEqual(400, result.StatusCode);
    }
    
    [TestMethod]
    public void AddRoleToUserPreconditionFailedStatusCode()
    {
        _userServiceMock
            .Setup(service => service.AssignRoleToUser(It.IsAny<long>(), It.IsAny<string>()))
            .Throws(new CannotAddItem(CannotAddItemMessage));
        _userController = new UserController(_userServiceMock.Object);
        PostUserRoleRequest request = new PostUserRoleRequest
        {
            Role = Random.Shared.Next().ToString()
        };

        ObjectResult result = _userController.PostUserRole(_user_1_example.Id, request) as ObjectResult;

        _userServiceMock.Verify();

        Assert.AreEqual(412, result.StatusCode);
    }
    
    private void SetupDefaultMock()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
    }

    private void SetupDefaultUsers()
    {
        Role role = new Administrator();
        _listOfRoles = new List<Role>() {};
        
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
        
        _listOfUsers = new List<User> { _user_1_example, _user_2_example};
    }
    
    private void SetupDefaultSession()
    {
        _session = new Session(); 
        _session.User = _user_1_example;
        _session.Id = new Guid();
    }
    
    private PageDataRequest DefaultPageDataRequest()
    {
        PageDataRequest request = new PageDataRequest();
        
        request.Page = Page;
        request.PageSize = PageSize;
        
        return request;
    }
}