using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using Moq;
using WebApi.Controllers;
using Exception = System.Exception;

namespace TestWebApi.Controllers;

[TestClass]
public class AdministratorControllerTest
{
    private const int OkStatusCode = 200;
    private const int CreatedStatusCode = 201;
    private const int NotFoundStatusCode = 404;
    private const int ConflictStatusCode = 409;
    private const int InternalServerErrorStatusCode = 500;

    private const string ErrorMessageWhenUserNotFound = "User does not exists";
    private const string ErrorMessageWhenUserAlreadyExists = "Member already exists";
    
    private const string Name =  "John";
    private const string Surname = "Doe";
    private const string Email = "john.doe@example.com";
    private const string Password = "Securepassword1@";
    private const string Photo = "address";
    private static readonly Guid Token = new Guid();


    private User _user; 
    private List<Role> _listOfRoles;
    private Administrator _administrator; 
    
    private Mock<IUserService> _userServiceMock;
    private AdministratorController _administratorController;
    private PostAdministratorRequest _postAdministratorRequest;
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _administratorController = new AdministratorController(_userServiceMock.Object);

        _listOfRoles = new List<Role>();

        _administrator = new Administrator();

        _listOfRoles.Add(_administrator); 
        
        _postAdministratorRequest = new PostAdministratorRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = Photo
        };

        _user = new User
        {
            Name = _postAdministratorRequest.Name,
            Email = _postAdministratorRequest.Email,
            Password = _postAdministratorRequest.Password,
            Surname = _postAdministratorRequest.Surname,
            Roles = _listOfRoles
        };
    }

    [TestMethod]
    public void CreateAdminValidRequest()
    {

        _userServiceMock.Setup(service => service.CreateUser(It.Is<User>(u =>
            u.Name == _user.Name &&
            u.Email == _user.Email &&
            u.Password == _user.Password &&
            u.Surname == _user.Surname 
        )));
        
        ObjectResult result = _administratorController.CreateAdministrator(_postAdministratorRequest) as ObjectResult;
        PostAdministratorResponse userResponse = result?.Value as PostAdministratorResponse;

        _userServiceMock.Verify();
        
        PostAdministratorResponse expectedResponse = new PostAdministratorResponse(_user);

        Assert.AreEqual(userResponse, expectedResponse);
        Assert.AreEqual(CreatedStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserAlreadyExists()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist(ErrorMessageWhenUserAlreadyExists));
        
        ObjectResult result = _administratorController.CreateAdministrator(_postAdministratorRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(ConflictStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUser_OtherException()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new Exception());
        
        ObjectResult result = _administratorController.CreateAdministrator(_postAdministratorRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(InternalServerErrorStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestDeleteUserOkStatusCode()
    {
        _userServiceMock.Setup(service => service.DeleteUser(It.IsAny<long>()));
        
        OkResult result = _administratorController.DeleteAdministrator(1) as OkResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(OkStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestDeleteUserNotFoundStatusCode()
    {
        _userServiceMock.Setup(service => service.DeleteUser(It.IsAny<long>()))
            .Throws(new ElementNotFound(ErrorMessageWhenUserNotFound));
        
        NotFoundResult result = _administratorController.DeleteAdministrator(1) as NotFoundResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
}