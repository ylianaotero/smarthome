using BusinessLogic.IServices;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.In;
using WebApi.Out;

namespace TestWebApi.Controllers;

[TestClass]
public class HomeOwnerControllerTest
{
    private const string ErrorMessageWhenInputIsInvalid = "Input not valid, try again";
    private const string ErrorMessageWhenElementAlreadyExists =  "Element already exists, try again";
    
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
    private CreateHomeOwnerRequest _createHomeOwnerRequest;
    private User _user; 
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);

        _homeOwnerController = new HomeOwnerController(_userServiceMock.Object);

        _listOfRoles = new List<Role>();

        _homeOwner = new HomeOwner();

        _listOfRoles.Add(_homeOwner); 
        
        _createHomeOwnerRequest = new CreateHomeOwnerRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl
        };
        
        _user = new User
        {
            Name = _createHomeOwnerRequest.Name,
            Email = _createHomeOwnerRequest.Email,
            Password = _createHomeOwnerRequest.Password,
            Surname = _createHomeOwnerRequest.Surname,
            Photo = _createHomeOwnerRequest.Photo,
            Roles = _listOfRoles
        };
    }

    [TestMethod]
    public void CreateUserValidRequest()
    {

        _userServiceMock.Setup(service => service.CreateUser(It.Is<User>(u =>
            u.Name == _user.Name &&
            u.Email == _user.Email &&
            u.Password == _user.Password &&
            u.Surname == _user.Surname &&
            u.Photo == _user.Photo
        )));

        var result = _homeOwnerController.CreateHomeOwner(_createHomeOwnerRequest) as ObjectResult;
        var userResponse = result?.Value as HomeOwnerResponse;

        _userServiceMock.Verify();
        
        Assert.IsTrue(
            result != null &&
            userResponse != null &&
            result.StatusCode == 201 &&
            _createHomeOwnerRequest.Name == userResponse.Name &&
            _createHomeOwnerRequest.Email == userResponse.Email &&
            _createHomeOwnerRequest.Surname == userResponse.Surname &&
            _createHomeOwnerRequest.Photo == userResponse.Photo &&
            userResponse.Roles.Count == 1
        );
    }
    
    [TestMethod]
    public void CreateUserInvalidRequest()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new InputNotValid(ErrorMessageWhenInputIsInvalid));
        
        var result = _homeOwnerController.CreateHomeOwner(_createHomeOwnerRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(400, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUnexpectedException()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new Exception());

        var result = _homeOwnerController.CreateHomeOwner(_createHomeOwnerRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(500, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUserAlreadyExists()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist(ErrorMessageWhenElementAlreadyExists));

        var result = _homeOwnerController.CreateHomeOwner(_createHomeOwnerRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(409, result.StatusCode);
    }
    
    
}