using BusinessLogic.IServices;
using Domain;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Out;
using LoginRequest = WebApi.In.LoginRequest;

namespace TestWebApi;

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
}