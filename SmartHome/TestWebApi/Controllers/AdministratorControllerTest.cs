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
    private const string ErrorMessageWhenUserAlreadyExists = "Member already exists";
    private const int NotFoundStatusCode = 404;
    private const int OkStatusCode = 200;
    private const string ErrorMessageWhenUserNotFound = "User does not exists";

    
    private const string Name =  "John";
    private const string Email = "john.doe@example.com";
    private const string Password = "Securepassword1@";
    private const string Surname = "Doe";
    private const int CreatedStatusCode = 201;
    private const int ConflictStatusCode = 409;
    private const int InternalServerErrorStatusCode = 500;
    
    private static readonly Guid Token = new Guid();


    private List<Role> _listOfRoles;
    private Administrator _administrator; 
    
    private Mock<IUserService> _userServiceMock;
    private AdministratorController _administratorController;
    private CreateAdminRequest _createAdminRequest;
    private User _user; 
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);

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
        
        _administratorController = new AdministratorController(_userServiceMock.Object);

        var result = _administratorController.CreateAdministrator(_createAdminRequest) as ObjectResult;
        var userResponse = result?.Value as AdminResponse;

        _userServiceMock.Verify();
        
        AdminResponse expectedResponse = new AdminResponse(_user);

        Assert.AreEqual(userResponse, expectedResponse);
        Assert.AreEqual(CreatedStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserAlreadyExists()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist(ErrorMessageWhenUserAlreadyExists));
        
        _administratorController = new AdministratorController(_userServiceMock.Object);
        
        var result = _administratorController.CreateAdministrator(_createAdminRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(ConflictStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUser_OtherException()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new Exception());
        
        _administratorController = new AdministratorController(_userServiceMock.Object);
        
        var result = _administratorController.CreateAdministrator(_createAdminRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(InternalServerErrorStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestDeleteUserOkStatusCode()
    {
        _userServiceMock.Setup(service => service.DeleteUser(It.IsAny<long>()));
        
        _administratorController = new AdministratorController(_userServiceMock.Object);
        
        var result = _administratorController.DeleteAdministrator(1) as OkResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(OkStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestDeleteUserNotFoundStatusCode()
    {
        _userServiceMock.Setup(service => service.DeleteUser(It.IsAny<long>()))
            .Throws(new ElementNotFound(ErrorMessageWhenUserNotFound));
        
        _administratorController = new AdministratorController(_userServiceMock.Object);
        
        var result = _administratorController.DeleteAdministrator(1) as NotFoundResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
}