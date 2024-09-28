using CustomExceptions;
using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.In;
using WebApi.Models.Out;

namespace TestWebApi.Controllers;

[TestClass]
public class HomeOwnerControllerTest
{
    private const string ErrorMessageWhenInputIsInvalid = "Input not valid, try again";
    private const string ErrorMessageWhenElementAlreadyExists =  "Element already exists, try again";
    private const string ErrorMessageWhenElementNotFound = "Element not found, try again";
    
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private const int HomeOwnerId = 1;
    private const string Name =  "John";
    private const string Email = "john.doe@example.com";
    private const string InvalidEmail = "invalid email";
    private const string Password = "Securepassword1@";
    private const string Surname = "Doe";
    private const int OkStatusCode = 200;
    private const int CreatedStatusCode = 201;
    private const int BadRequestStatusCode = 400;
    private const int NotFoundStatusCode = 404;
    private const int ConflictStatusCode = 409;
    private const int InternalServerErrorStatusCode = 500;

    private List<Role> _listOfRoles;
    private HomeOwner _homeOwner; 
    
    private Mock<IUserService> _userServiceMock;
    private HomeOwnerController _homeOwnerController;
    private CreateHomeOwnerRequest _createHomeOwnerRequest;
    private UpdateHomeOwnerRequest _updateHomeOwnerRequest;
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
        
        _updateHomeOwnerRequest = new UpdateHomeOwnerRequest
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
            result.StatusCode == CreatedStatusCode &&
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
        
        Assert.AreEqual(BadRequestStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUnexpectedException()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new Exception());

        var result = _homeOwnerController.CreateHomeOwner(_createHomeOwnerRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(InternalServerErrorStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUserAlreadyExists()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist(ErrorMessageWhenElementAlreadyExists));

        var result = _homeOwnerController.CreateHomeOwner(_createHomeOwnerRequest) as ObjectResult;
        
        _userServiceMock.Verify();
        
        Assert.AreEqual(ConflictStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestUpdateHomeOwnerOkResponse()
    {
        _userServiceMock.Setup(service => service.UpdateUser(It.IsAny<long>(), It.Is<User>(u =>
            u.Name == _updateHomeOwnerRequest.Name &&
            u.Email == _updateHomeOwnerRequest.Email &&
            u.Password == _updateHomeOwnerRequest.Password &&
            u.Surname == _updateHomeOwnerRequest.Surname &&
            u.Photo == _updateHomeOwnerRequest.Photo
        ))).Verifiable();

        var result = _homeOwnerController.UpdateHomeOwner(1, _updateHomeOwnerRequest) as ObjectResult;
        var userResponse = result?.Value as HomeOwnerResponse;
        Assert.IsTrue(
            result != null && 
            userResponse != null && 
            result.StatusCode == OkStatusCode &&
            userResponse.Name == _updateHomeOwnerRequest.Name && 
            userResponse.Email == _updateHomeOwnerRequest.Email &&
            userResponse.Surname == _updateHomeOwnerRequest.Surname &&
            userResponse.Photo == _updateHomeOwnerRequest.Photo
        );

        _userServiceMock.Verify();
    }
    
    //REVISAR
    [TestMethod]
    public void UpdateHomeOwnerInvalidRequest()
    {
        var updateHomeOwnerRequest = new UpdateHomeOwnerRequest
        {
            Name = "John",
            Email = "john.doe@example.com",
            Password = "newPassword",
            Surname = "Doe",
            Photo = "https://example.com/photo.jpg"
        };

        _userServiceMock
            .Setup(service => service.UpdateUser(It.IsAny<long>(), It.IsAny<User>()))
            .Throws(new InputNotValid(ErrorMessageWhenInputIsInvalid));

        var result = _homeOwnerController.UpdateHomeOwner(1, updateHomeOwnerRequest) as ObjectResult;

        Assert.AreEqual(BadRequestStatusCode, result.StatusCode);
    }

    [TestMethod]
    public void TestUpdateHomeOwnerElementNotFound()
    {
        _userServiceMock
            .Setup(service => service.UpdateUser(It.IsAny<long>(), It.IsAny<User>()))
            .Throws(new ElementNotFound(ErrorMessageWhenElementNotFound));

        var result = _homeOwnerController.UpdateHomeOwner(HomeOwnerId, _updateHomeOwnerRequest) as ObjectResult;

        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
}