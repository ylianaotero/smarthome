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
    private const string CannotFinItemMessage = "Cannot find item in list";
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    
    private const string ErrorMessageWhenInputIsInvalid = "Input not valid, try again";
    private const string ErrorMessageWhenElementAlreadyExists =  "Element already exists, try again";
    private const string ErrorMessageWhenElementNotFound = "Element not found, try again";

    private const string Role = "Administrator";
    
    private List<Role> _listOfRoles;
    private Session _session; 
    
    private const int OkStatusCode = 200;
    private const int CreatedStatusCode = 201;
    private const int NotFoundStatusCode = 404;
    private const int ConflictStatusCode = 409;
    private const int BadRequestStatusCode = 400;
    private const int InternalServerErrorStatusCode = 500;
    
    private Mock<IUserService> _userServiceMock;
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

        _userController = new UserController(_userServiceMock.Object);
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
        _userServiceMock.Setup(service => service.IsAdmin(_session.User.Email)).Returns(true); 
        _userServiceMock
            .Setup(service => service.GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers); 
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
            Role = Role
        };
        List<User> listOfUsers =
        [
            _user_3_example,
        ];
        _userServiceMock
            .Setup(service => service.GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(listOfUsers); 
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
            .Setup(service => service.GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Throws(new CannotFindItemInList(CannotFinItemMessage));
        
        _userController.GetUsers(new GetUsersRequest(), DefaultPageDataRequest());
    }
    
    private PageDataRequest DefaultPageDataRequest()
    {
        PageDataRequest request = new PageDataRequest();
        
        request.Page = Page;
        request.PageSize = PageSize;
        
        return request;
    }
    
    [TestMethod]
    public void CreateCompanyOwnerValidRequest()
    {
        PostCompanyOwnerRequest postCompanyOwnerRequest = new PostCompanyOwnerRequest()
        {
            Name = Name,
            Surname = Surname,
                Email = Email1, 
                Password = Password
        };

        _userServiceMock.Setup(service => service.CreateUser(It.Is<User>(u =>
            u.Name == Name &&
            u.Email == Email1 &&
            u.Password == Password &&
            u.Surname == Surname
        )));

        ObjectResult result = _userController.CreateCompanyOwner(postCompanyOwnerRequest) as ObjectResult;
        PostCompanyOwnerResponse userResponse = result?.Value as PostCompanyOwnerResponse;

        _userServiceMock.Verify();
        
        Assert.IsTrue(
            result != null &&
            userResponse != null &&
            result.StatusCode == CreatedStatusCode &&
            postCompanyOwnerRequest.Name == userResponse.Name &&
            postCompanyOwnerRequest.Email == userResponse.Email &&
            postCompanyOwnerRequest.Surname == userResponse.Surname &&
            userResponse.Roles.Count == 1
        );
    }
    
    [TestMethod]
    public void CreateCompanyOwnerInvalidRequest()
    {
        PostCompanyOwnerRequest postCompanyOwnerRequest = new PostCompanyOwnerRequest()
        {
            Name = Name,
            Surname = Surname,
            Email = Email1, 
            Password = Password
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new InputNotValid(ErrorMessageWhenInputIsInvalid));
        
        ObjectResult result = _userController.CreateCompanyOwner(postCompanyOwnerRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(BadRequestStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUserAlreadyExists()
    {
        PostCompanyOwnerRequest postCompanyOwnerRequest = new PostCompanyOwnerRequest()
        {
            Name = Name,
            Surname = Surname,
            Email = Email1, 
            Password = Password
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist(ErrorMessageWhenElementAlreadyExists));

        ObjectResult result = _userController.CreateCompanyOwner(postCompanyOwnerRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(ConflictStatusCode, result.StatusCode);
    }
    
    
}