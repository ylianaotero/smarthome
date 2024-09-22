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
public class HomeOwnerControllerTest
{
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private const string Name =  "John";
    private const string Email = "john.doe@example.com";
    private const string InvalidEmail = "invalid email";
    private const string Password = "Securepassword1@";
    private const string Surname = "Doe";

    private List<Role> _listOfRoles;
    private HomeOwner _homeOwner; 
    
    private Mock<IUserService> _userServiceMock;
    private HomeOwnerController _homeOwnerController;
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);

        _homeOwnerController = new HomeOwnerController(_userServiceMock.Object);

        _listOfRoles = new List<Role>();

        _homeOwner = new HomeOwner();

        _listOfRoles.Add(_homeOwner); 
    }

    [TestMethod]
    public void CreateUserValidRequest()
    {
        var createUserRequest = new CreateHomeOwnerRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl
        };

        var user = new User
        {
            Name = createUserRequest.Name,
            Email = createUserRequest.Email,
            Password = createUserRequest.Password,
            Surname = createUserRequest.Surname,
            Photo = createUserRequest.Photo,
            Roles = _listOfRoles
        };

        _userServiceMock.Setup(service => service.CreateUser(It.Is<User>(u =>
            u.Name == user.Name &&
            u.Email == user.Email &&
            u.Password == user.Password &&
            u.Surname == user.Surname &&
            u.Photo == user.Photo
        )));

        var result = _homeOwnerController.CreateHomeOwner(createUserRequest) as OkObjectResult;
        var userResponse = result?.Value as HomeOwnerResponse;

        _userServiceMock.Verify(service => service.CreateUser(It.Is<User>(u =>
            u.Name == user.Name &&
            u.Email == user.Email &&
            u.Password == user.Password &&
            u.Surname == user.Surname &&
            u.Photo == user.Photo
        )), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsNotNull(userResponse);
        Assert.AreEqual(createUserRequest.Name, userResponse.Name);
        Assert.AreEqual(createUserRequest.Email, userResponse.Email);
        Assert.AreEqual(createUserRequest.Surname, userResponse.Surname);
        Assert.AreEqual(createUserRequest.Photo, userResponse.Photo);
        Assert.AreEqual(1, userResponse.Roles.Count);
    }
    
    [TestMethod]
    public void CreateUserInvalidRequest()
    {
        var createUserRequest = new CreateHomeOwnerRequest
        {
            Name = Name,
            Email = InvalidEmail,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new InputNotValid("Input not valid, try again"));
        
        var result = _homeOwnerController.CreateHomeOwner(createUserRequest) as ObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(400, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUnexpectedException()
    {
        var createUserRequest = new CreateHomeOwnerRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new Exception("Unexpected error"));

        var result = _homeOwnerController.CreateHomeOwner(createUserRequest) as ObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(500, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUserAlreadyExists()
    {
        var createUserRequest = new CreateHomeOwnerRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist("Element already exists, try again"));

        var result = _homeOwnerController.CreateHomeOwner(createUserRequest) as ObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(409, result.StatusCode);
    }

    
    
    
    
}