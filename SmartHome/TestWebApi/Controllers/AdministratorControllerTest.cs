using BusinessLogic.Exceptions;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.In;
using WebApi.Models.Out;
using Exception = System.Exception;

namespace TestWebApi.Controllers;

[TestClass]
public class AdministratorControllerTest
{
    private const string ErrorMessageWhenInputIsInvalid = "Input not valid, try again";
    private const string ErrorMessageWhenUserAlreadyExists = "User already exists";
    private const string ErrorMessageWhenCannotFindUser =  "Cannot find user";
    
    private const string Name =  "John";
    private const string Email = "john.doe@example.com";
    private const string Password = "Securepassword1@";
    private const string Surname = "Doe";
    private static readonly Guid Token = new Guid();


    private List<Role> _listOfRoles;
    private Administrator _administrator; 
    
    private Mock<IUserService> _userServiceMock;
    private Mock<ISessionService> _sessionServiceMock; 
    private AdministratorController _administratorController;
    private CreateAdminRequest _createAdminRequest;
    private User _user; 
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);

        _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict); 

        _listOfRoles = new List<Role>();

        _administrator = new Administrator();

        _listOfRoles.Add(_administrator); 
        
        _createAdminRequest = new CreateAdminRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname
        };

        _user = new User
        {
            Name = _createAdminRequest.Name,
            Email = _createAdminRequest.Email,
            Password = _createAdminRequest.Password,
            Surname = _createAdminRequest.Surname,
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

        _sessionServiceMock.Setup(service => service.GetUser(Token)).Returns(_user); 
        
        _userServiceMock.Setup(service => service.IsAdmin(_user.Email)).Returns(true); 
        
        _administratorController = new AdministratorController(_userServiceMock.Object, _sessionServiceMock.Object);

        var result = _administratorController.CreateUser(_createAdminRequest, Token) as ObjectResult;
        var userResponse = result?.Value as AdminResponse;

        _userServiceMock.Verify();
        _sessionServiceMock.Verify();
        
        var expectedResponse = new
        {
            StatusCode = 201,
            Name = _createAdminRequest.Name,
            Email = _createAdminRequest.Email,
            Surname = _createAdminRequest.Surname
        };

        var actualResponse = new
        {
            StatusCode = result.StatusCode,
            Name = userResponse.Name,
            Email = userResponse.Email,
            Surname = userResponse.Surname
        };

        Assert.IsTrue(expectedResponse.StatusCode == actualResponse.StatusCode &&
                      expectedResponse.Name == actualResponse.Name &&
                      expectedResponse.Email == actualResponse.Email &&
                      expectedResponse.Surname == actualResponse.Surname);
    }
    
    [TestMethod]
    public void CreateUserInvalidRequest()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new InputNotValid(ErrorMessageWhenInputIsInvalid));

        _sessionServiceMock.Setup(service => service.GetUser(Token)).Returns(_user); 
        
        _userServiceMock.Setup(service => service.IsAdmin(_user.Email)).Returns(true); 
        
        _administratorController = new AdministratorController(_userServiceMock.Object, _sessionServiceMock.Object);
        
        var result = _administratorController.CreateUser(_createAdminRequest, Token) as ObjectResult;
        
        _userServiceMock.Verify();
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(400, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserAlreadyExists()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist(ErrorMessageWhenUserAlreadyExists));

        _sessionServiceMock.Setup(service => service.GetUser(Token)).Returns(_user); 
        
        _userServiceMock.Setup(service => service.IsAdmin(_user.Email)).Returns(true); 
        
        _administratorController = new AdministratorController(_userServiceMock.Object, _sessionServiceMock.Object);
        
        var result = _administratorController.CreateUser(_createAdminRequest, Token) as ObjectResult;
        
        _userServiceMock.Verify();
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(409, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUser_OtherException()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new Exception());

        _sessionServiceMock.Setup(service => service.GetUser(Token)).Returns(_user); 
        
        _userServiceMock.Setup(service => service.IsAdmin(_user.Email)).Returns(true); 
        
        _administratorController = new AdministratorController(_userServiceMock.Object, _sessionServiceMock.Object);
        
        var result = _administratorController.CreateUser(_createAdminRequest, Token) as ObjectResult;
        
        _userServiceMock.Verify();
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(500, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUser_IsNotAdmin()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new Exception());

        _sessionServiceMock.Setup(service => service.GetUser(Token)).Returns(_user); 
        
        _userServiceMock.Setup(service => service.IsAdmin(_user.Email)).Returns(false); 
        
        _administratorController = new AdministratorController(_userServiceMock.Object, _sessionServiceMock.Object);
        
        var result = _administratorController.CreateUser(_createAdminRequest, Token) as ObjectResult;
        
        _userServiceMock.Verify();
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(403, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUser_TokenIsInvalid()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new Exception());

        _sessionServiceMock.Setup(service => service.GetUser(Token)).Throws(new CannotFindItemInList(ErrorMessageWhenCannotFindUser )); 
        
        _userServiceMock.Setup(service => service.IsAdmin(_user.Email)).Returns(true); 
        
        _administratorController = new AdministratorController(_userServiceMock.Object, _sessionServiceMock.Object);
        
        var result = _administratorController.CreateUser(_createAdminRequest, Token) as ObjectResult;
        
        _userServiceMock.Verify();
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(401, result.StatusCode);
    }
}