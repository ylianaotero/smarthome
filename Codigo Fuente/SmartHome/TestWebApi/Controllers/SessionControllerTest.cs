using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using Moq;
using WebApi.Controllers;

namespace TestWebApi.Controllers;

[TestClass]
public class SessionControllerTest
{
    private const string ErrorMessageWhenCannotFindElement = "Input not valid, try again";
    private const string Email = "john.doe@example.com";
    private const string Password = "Securepassword1@";
    private const int NotFoundStatusCode = 404;
    private const int OkStatusCode = 200;
    
    private List<Role> _listOfRoles;
    private HomeOwner _homeOwner; 
    private Mock<ISessionService> _sessionServiceMock;
    private SessionController _sessionController;
    private LoginRequest _createLoginRequest;
    private Session _session; 
    
    [TestInitialize]
    public void TestInitialize()
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

        OkObjectResult result = _sessionController.LogIn(_createLoginRequest) as OkObjectResult;
        LoginResponse response = result?.Value as LoginResponse;
        
        _sessionServiceMock.VerifyAll();
        
        Assert.IsTrue(
            result != null &&
            result.StatusCode == OkStatusCode &&
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
        
        ObjectResult result = _sessionController.LogIn(_createLoginRequest) as ObjectResult;
        
        _sessionServiceMock.VerifyAll();
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
}