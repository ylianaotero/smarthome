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
    private Mock<IUserService> _userServiceMock;
    private HomeOwnerController _homeOwnerController;
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);

        _homeOwnerController = new HomeOwnerController(_userServiceMock.Object);
    }

    [TestMethod]
    public void CreateUserValidRequest()
    {
        var createUserRequest = new CreateHomeOwnerRequest
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Securepassword1@",
            Surname = "Doe"
        };

        var user = new User
        {
            Name = createUserRequest.Name,
            Email = createUserRequest.Email,
            Password = createUserRequest.Password,
            Surname = createUserRequest.Surname,
            Roles = new List<Role>()
        };

        _userServiceMock.Setup(service => service.CreateUser(It.Is<User>(u =>
            u.Name == user.Name &&
            u.Email == user.Email &&
            u.Password == user.Password &&
            u.Surname == user.Surname
        )));

        var result = _homeOwnerController.CreateUser(createUserRequest) as OkObjectResult;
        var userResponse = result?.Value as HomeOwnerResponse;

        _userServiceMock.Verify(service => service.CreateUser(It.Is<User>(u =>
            u.Name == user.Name &&
            u.Email == user.Email &&
            u.Password == user.Password &&
            u.Surname == user.Surname
        )), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsNotNull(userResponse);
        Assert.AreEqual(createUserRequest.Name, userResponse.Name);
        Assert.AreEqual(createUserRequest.Email, userResponse.Email);
        Assert.AreEqual(createUserRequest.Surname, userResponse.Surname);
    }
    
    [TestMethod]
    public void CreateUserInvalidRequest()
    {
        var createUserRequest = new CreateHomeOwnerRequest
        {
            Name = "John Doe",
            Email = "invalid email",
            Password = "Securepassword1@",
            Surname = "Doe"
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new InputNotValid("Input not valid, try again"));
        
        var result = _homeOwnerController.CreateUser(createUserRequest) as ObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(400, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUnexpectedException()
    {
        var createUserRequest = new CreateHomeOwnerRequest
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Securepassword1@",
            Surname = "Doe"
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new Exception("Unexpected error"));

        var result = _homeOwnerController.CreateUser(createUserRequest) as ObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(500, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUserAlreadyExists()
    {
        var createUserRequest = new CreateHomeOwnerRequest
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Securepassword1@",
            Surname = "Doe"
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist("Element already exists, try again"));

        var result = _homeOwnerController.CreateUser(createUserRequest) as ObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(409, result.StatusCode);
    }

    
    
    
    
}