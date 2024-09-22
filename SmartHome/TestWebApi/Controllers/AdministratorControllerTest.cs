using BusinessLogic.IServices;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.In;
using WebApi.Out;

namespace TestWebApi;

[TestClass]
public class AdministratorControllerTest
{
    private const string Name =  "John";
    private const string Email = "john.doe@example.com";
    private const string InvalidEmail = "invalid email";
    private const string Password = "Securepassword1@";
    private const string Surname = "Doe";

    private List<Role> _listOfRoles;
    private Administrator _administrator; 
    
    private Mock<IUserService> _userServiceMock;
    private AdministratorController _administratorController;
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);

        _listOfRoles = new List<Role>();

        _administrator = new Administrator();

        _listOfRoles.Add(_administrator); 
    }

    [TestMethod]
    public void CreateAdminValidRequest()
    {
        var createAdminRequest = new CreateAdminRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname
        };

        var user = new User
        {
            Name = createAdminRequest.Name,
            Email = createAdminRequest.Email,
            Password = createAdminRequest.Password,
            Surname = createAdminRequest.Surname,
            Roles = _listOfRoles
        };

        _userServiceMock.Setup(service => service.CreateUser(It.Is<User>(u =>
            u.Name == user.Name &&
            u.Email == user.Email &&
            u.Password == user.Password &&
            u.Surname == user.Surname 
        )));
        
        _administratorController = new AdministratorController(_userServiceMock.Object);

        var result = _administratorController.CreateUser(createAdminRequest) as ObjectResult;
        var userResponse = result?.Value as AdminResponse;

        _userServiceMock.Verify();
        
        Assert.AreEqual(201, result.StatusCode);
        Assert.IsNotNull(userResponse);
        Assert.AreEqual(createAdminRequest.Name, userResponse.Name);
        Assert.AreEqual(createAdminRequest.Email, userResponse.Email);
        Assert.AreEqual(createAdminRequest.Surname, userResponse.Surname);
    }
    
    [TestMethod]
    public void CreateUserInvalidRequest()
    {
        var createUserRequest = new CreateAdminRequest
        {
            Name = Name,
            Email = InvalidEmail,
            Password = Password,
            Surname = Surname
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new InputNotValid("Input not valid, try again"));
        
        _administratorController = new AdministratorController(_userServiceMock.Object);
        
        var result = _administratorController.CreateUser(createUserRequest) as ObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(400, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserAlreadyExists()
    {
        var createUserRequest = new CreateAdminRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist("User already exists"));
        
        _administratorController = new AdministratorController(_userServiceMock.Object);
        
        var result = _administratorController.CreateUser(createUserRequest) as ObjectResult;
        
        Assert.AreEqual(409, result.StatusCode);
    }
}