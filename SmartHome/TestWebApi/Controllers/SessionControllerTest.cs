using BusinessLogic.IServices;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Out;
using LoginRequest = WebApi.In.LoginRequest;

namespace TestWebApi.Controllers;

[TestClass]
public class SessionControllerTest
{
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private const string Name =  "John";
    private const string Email = "john.doe@example.com";
    private const string InvalidEmail = "invalid email";
    private const string Password = "Securepassword1@";
    private const string Surname = "Doe";
    
    private List<Role> _listOfRoles;
    private HomeOwner _homeOwner; 
    
    private Mock<ISessionService> _sessionServiceMock;
    private SessionController _sessionController;
    
    [TestInitialize]
    public void SetUp()
    {
        _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);

        _sessionController = new SessionController(_sessionServiceMock.Object);
        
        _listOfRoles = new List<Role>();

        _homeOwner = new HomeOwner();

        _listOfRoles.Add(_homeOwner); 
    }
    
    [TestMethod]
    public void LoginValidRequest()
    {
        LoginRequest createLoginRequest = new LoginRequest()
        {
            Email = Email,
            Password = Password
        };

        var user = new User
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl,
            Roles = _listOfRoles
        };
        
        var session = new Session
        {
            Id = Guid.NewGuid(), 
            User = new User { Email = Email }
        };
        

        _sessionServiceMock
            .Setup(s => s.LogIn(Email, Password))
            .Returns(session);

        var result = _sessionController.LogIn(createLoginRequest) as OkObjectResult;
        var response = result?.Value as LoginResponse;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsNotNull(response);
        Assert.AreEqual(session.Id, response.Token);
    }
    
    
    [TestMethod]
    public void LoginInvalidRequest()
    {
        var createLoginRequest = new LoginRequest
        {
            Email = Email,
            Password = Password
        };

        _sessionServiceMock
            .Setup(s => s.LogIn(Email, Password))
            .Throws(new CannotFindItemInList("Input not valid, try again"));
        
        var result = _sessionController.LogIn(createLoginRequest) as ObjectResult;

        Assert.IsNotNull(result, "The result should not be null.");
        Assert.AreEqual(404, result.StatusCode);
    }
    
    [TestMethod]
    public void LoginError()
    {
        var createLoginRequest = new LoginRequest
        {
            Email = Email,
            Password = Password
        };

        _sessionServiceMock
            .Setup(s => s.LogIn(Email, Password))
            .Throws(new Exception("Error"));
        
        var result = _sessionController.LogIn(createLoginRequest) as ObjectResult;

        Assert.IsNotNull(result, "The result should not be null.");
        Assert.AreEqual(500, result.StatusCode);
    }
}