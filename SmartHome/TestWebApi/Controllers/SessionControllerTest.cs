using Domain;
using Domain.Exceptions.GeneralExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.Out;
using LoginRequest = WebApi.Models.In.LoginRequest;

namespace TestWebApi.Controllers;

[TestClass]
public class SessionControllerTest
{
    private const string ErrorMessageWhenCannotFindElement = "Input not valid, try again";
    
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    
    private const string Name =  "John";
    private const string Email = "john.doe@example.com";
    private const string Password = "Securepassword1@";
    private const string Surname = "Doe";
    
    private List<Role> _listOfRoles;
    private HomeOwner _homeOwner; 
    
    private Mock<ISessionService> _sessionServiceMock;
    private SessionController _sessionController;

    private LoginRequest _createLoginRequest;
    private User _user;
    private Session _session; 
    
    [TestInitialize]
    public void SetUp()
    {
        _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);

        _sessionController = new SessionController(_sessionServiceMock.Object);
        
        _listOfRoles = new List<Role>();

        _homeOwner = new HomeOwner();

        _listOfRoles.Add(_homeOwner); 
        
        _createLoginRequest = new LoginRequest()
        {
            Email = Email,
            Password = Password
        };

        _user = new User
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl,
            Roles = _listOfRoles
        };
        
        _session = new Session
        {
            Id = Guid.NewGuid(), 
            User = new User { Email = Email }
        };
    }
    
    [TestMethod]
    public void LoginValidRequest()
    {
        _sessionServiceMock
            .Setup(s => s.LogIn(Email, Password))
            .Returns(_session);

        var result = _sessionController.LogIn(_createLoginRequest) as OkObjectResult;
        var response = result?.Value as LoginResponse;
        
        _sessionServiceMock.Verify();
        
        Assert.IsTrue(
            result != null &&
            result.StatusCode == 200 &&
            response != null &&
            response.Token == _session.Id
        );
    }
    
    
    [TestMethod]
    public void LoginInvalidRequest()
    {
        _sessionServiceMock
            .Setup(s => s.LogIn(Email, Password))
            .Throws(new CannotFindItemInList(ErrorMessageWhenCannotFindElement));
        
        var result = _sessionController.LogIn(_createLoginRequest) as ObjectResult;
        
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(404, result.StatusCode);
    }
    
    [TestMethod]
    public void LoginError()
    {
        _sessionServiceMock
            .Setup(s => s.LogIn(Email, Password))
            .Throws(new Exception("Error"));
        
        var result = _sessionController.LogIn(_createLoginRequest) as ObjectResult;
        
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(500, result.StatusCode);
    }
    
    [TestMethod]
    public void LoginNullRequest()
    {
        var result = _sessionController.LogIn(null) as ObjectResult;
        
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(400, result.StatusCode);
    }
    
    [TestMethod]
    public void LoginNullEmail()
    {
        LoginRequest _newLoginRequest = new LoginRequest()
        {
            Email = "",
            Password = Password
        };
        
        var result = _sessionController.LogIn(_newLoginRequest) as ObjectResult;
        
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(400, result.StatusCode);
    }
    
    [TestMethod]
    public void LoginNullPassword()
    {
        LoginRequest _newLoginRequest = new LoginRequest()
        {
            Email = Email,
            Password = ""
        };
        
        var result = _sessionController.LogIn(_newLoginRequest) as ObjectResult;
        
        _sessionServiceMock.Verify();
        
        Assert.AreEqual(400, result.StatusCode);
    }
}