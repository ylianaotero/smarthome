using BusinessLogic.IServices;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.In;
using WebApi.Out;

namespace TestWebApi;

[TestClass]
public class UserControllerTest
{
    private Mock<IUserService> _userServiceMock;
    private UserController _userController;
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);

        _userController = new UserController(_userServiceMock.Object);
    }

    [TestMethod]
    public void CreateUserValidRequest()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
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
            Roles = new List<Role>() // Assuming roles are empty or mock as needed
        };

        _userServiceMock.Setup(service => service.CreateUser(It.Is<User>(u =>
            u.Name == user.Name &&
            u.Email == user.Email &&
            u.Password == user.Password &&
            u.Surname == user.Surname
        )));

        // Act
        var result = _userController.CreateUser(createUserRequest) as OkObjectResult;
        var userResponse = result?.Value as UserResponse;

        // Assert
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
}