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
    private const string RoleAlreadyExistMessage = "Role already exist";
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private const string AdministratorRole = "Administrator";
    private const string HomeOwnerRole = "HomeOwner";
    private const int OkStatusCode = 200;
    private const int NotFoundStatusCode = 404;
    private const int BadRequestStatusCode = 400;
    private const int PreconditionFailedStatusCode = 412;
    private const int ConflictStatusCode = 409;
    
    
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
        
        ObjectResult result = _userController
            .GetUsers(new GetUsersRequest(), DefaultPageDataRequest()) as NotFoundObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }

    [TestMethod]
    public void AddRoleToUserOkStatusCode()
    {
        _userServiceMock
            .Setup(service => service.AssignRoleToUser(It.IsAny<long>(), It.IsAny<string>()))
            .Returns(_user_1_example);
        _userController = new UserController(_userServiceMock.Object);
        PostUserRoleRequest request = new PostUserRoleRequest
        {
            Role = HomeOwnerRole,
        };
        
        ObjectResult result = _userController.PostUserRole(_user_1_example.Id, request) as OkObjectResult;

        _userServiceMock.Verify();

        Assert.AreEqual(OkStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void AddRoleToUserOkResponse()
    {
        _userServiceMock
            .Setup(service => service.AssignRoleToUser(It.IsAny<long>(), It.IsAny<string>()))
            .Returns(_user_1_example);
        _userController = new UserController(_userServiceMock.Object);
        PostUserRoleRequest request = new PostUserRoleRequest
        {
            Role = HomeOwnerRole,
        };
        GetUserResponse expectedResponse = new GetUserResponse(_user_1_example);
        
        ObjectResult result = _userController.PostUserRole(_user_1_example.Id, request) as OkObjectResult;
        GetUserResponse getUserResponse = result.Value as GetUserResponse;

        _userServiceMock.Verify();

        Assert.AreEqual(expectedResponse, getUserResponse);
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

        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
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

        Assert.AreEqual(BadRequestStatusCode, result.StatusCode);
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

        Assert.AreEqual(PreconditionFailedStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void AddRoleToUserConflictStatusCode()
    {
        _userServiceMock
            .Setup(service => service.AssignRoleToUser(It.IsAny<long>(), It.IsAny<string>()))
            .Throws(new ElementAlreadyExist(RoleAlreadyExistMessage));
        _userController = new UserController(_userServiceMock.Object);
        PostUserRoleRequest request = new PostUserRoleRequest
        {
            Role = HomeOwnerRole,
        };
        
        ObjectResult result = _userController.PostUserRole(_user_1_example.Id, request) as ConflictObjectResult;

        _userServiceMock.Verify();

        Assert.AreEqual(ConflictStatusCode, result.StatusCode);
    }

    [TestMethod]
    public void GetUserByIdOkStatusCode()
    {
        _userServiceMock
            .Setup(service => service.GetUserById(It.IsAny<long>()))
            .Returns(_user_1_example);
        _userController = new UserController(_userServiceMock.Object);
        
        ObjectResult result = _userController.GetUser(_user_1_example.Id) as OkObjectResult;
        
        Assert.AreEqual(OkStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void GetUserByIdNotFoundStatusCode()
    {
        _userServiceMock
            .Setup(service => service.GetUserById(It.IsAny<long>()))
            .Throws(new ElementNotFound(CannotFindItemMessage));
        _userController = new UserController(_userServiceMock.Object);

        ObjectResult result = _userController.GetUser(_user_1_example.Id) as NotFoundObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }

    [TestMethod]
    public void GetUserByIdOkResponse()
    {
        _userServiceMock
            .Setup(service => service.GetUserById(It.IsAny<long>()))
            .Returns(_user_1_example);
        _userController = new UserController(_userServiceMock.Object);
        GetUserResponse expectedResponse = new GetUserResponse(_user_1_example);
        
        ObjectResult result = _userController.GetUser(_user_1_example.Id) as OkObjectResult;
        GetUserResponse getUserResponse = result.Value as GetUserResponse;
        
        Assert.AreEqual(expectedResponse, getUserResponse);
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
        _session.Id = Guid.NewGuid();
    }
    
    private static PageDataRequest DefaultPageDataRequest()
    {
        PageDataRequest request = new PageDataRequest();
        
        request.Page = Page;
        request.PageSize = PageSize;
        
        return request;
    }
}