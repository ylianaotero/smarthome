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
    private const int NotFoundStatusCode = 404;
    private const int ConflictStatusCode = 409;
    private const int BadRequestStatusCode = 400;

    private User _user; 
    private List<Role> _listOfRoles;
    private HomeOwner _homeOwner; 
    
    private Mock<IUserService> _userServiceMock;
    private HomeOwnerController _homeOwnerController;
    private PostHomeOwnerRequest _postHomeOwnerRequest;
    private PutHomeOwnerRequest _putHomeOwnerRequest;
    
    [TestInitialize]
    public void TestInitialize()
    {
        SetupDefaultMocks();
        SetupDefaultRoles();
        SetupDefaultRequests();
        SetupDefaultUser();
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

        ObjectResult result = _homeOwnerController.CreateHomeOwner(_postHomeOwnerRequest) as ObjectResult;
        PostHomeOwnerResponse userResponse = result?.Value as PostHomeOwnerResponse;

        _userServiceMock.VerifyAll();
        
        Assert.IsTrue(
            result != null &&
            userResponse != null &&
            result.StatusCode == CreatedStatusCode &&
            _postHomeOwnerRequest.Name == userResponse.Name &&
            _postHomeOwnerRequest.Email == userResponse.Email &&
            _postHomeOwnerRequest.Surname == userResponse.Surname &&
            _postHomeOwnerRequest.Photo == userResponse.Photo &&
            userResponse.Roles.Count == 1
        );
    }
    
    [TestMethod]
    public void CreateUserInvalidRequest()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new InputNotValid(ErrorMessageWhenInputIsInvalid));
        
        ObjectResult result = _homeOwnerController.CreateHomeOwner(_postHomeOwnerRequest) as ObjectResult;
        
        _userServiceMock.VerifyAll();
        
        Assert.AreEqual(BadRequestStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUserAlreadyExists()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist(ErrorMessageWhenElementAlreadyExists));

        ObjectResult result = _homeOwnerController.CreateHomeOwner(_postHomeOwnerRequest) as ObjectResult;
        
        _userServiceMock.VerifyAll();
        
        Assert.AreEqual(ConflictStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestUpdateHomeOwnerOkResponse()
    {
        _userServiceMock.Setup(service => service.UpdateUser(It.IsAny<long>(), It.Is<User>(u =>
            u.Name == _putHomeOwnerRequest.Name &&
            u.Email == _putHomeOwnerRequest.Email &&
            u.Password == _putHomeOwnerRequest.Password &&
            u.Surname == _putHomeOwnerRequest.Surname &&
            u.Photo == _putHomeOwnerRequest.Photo
        ))).Verifiable();

        ObjectResult result = _homeOwnerController.UpdateHomeOwner(1, _putHomeOwnerRequest) as ObjectResult;
        PostHomeOwnerResponse userResponse = result?.Value as PostHomeOwnerResponse;
        Assert.IsTrue(
            result != null && 
            userResponse != null && 
            result.StatusCode == OkStatusCode &&
            userResponse.Name == _putHomeOwnerRequest.Name && 
            userResponse.Email == _putHomeOwnerRequest.Email &&
            userResponse.Surname == _putHomeOwnerRequest.Surname &&
            userResponse.Photo == _putHomeOwnerRequest.Photo
        );

        _userServiceMock.VerifyAll();
    }
    
    [TestMethod]
    public void UpdateHomeOwnerInvalidRequest()
    {
        PutHomeOwnerRequest updateHomeOwnerRequest = new PutHomeOwnerRequest
        {
            Name = Name,
            Email = InvalidEmail,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl
        };

        _userServiceMock
            .Setup(service => service.UpdateUser(It.IsAny<long>(), It.IsAny<User>()))
            .Throws(new InputNotValid(ErrorMessageWhenInputIsInvalid));

        ObjectResult result = _homeOwnerController.UpdateHomeOwner(1, updateHomeOwnerRequest) as ObjectResult;

        Assert.AreEqual(BadRequestStatusCode, result.StatusCode);
    }

    [TestMethod]
    public void TestUpdateHomeOwnerElementNotFound()
    {
        _userServiceMock
            .Setup(service => service.UpdateUser(It.IsAny<long>(), It.IsAny<User>()))
            .Throws(new ElementNotFound(ErrorMessageWhenElementNotFound));

        ObjectResult result = _homeOwnerController.UpdateHomeOwner(HomeOwnerId, _putHomeOwnerRequest) as ObjectResult;

        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
    private void SetupDefaultMocks()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _homeOwnerController = new HomeOwnerController(_userServiceMock.Object);
    }
    
    private void SetupDefaultRoles()
    {
        _listOfRoles = new List<Role>();
        _homeOwner = new HomeOwner();
        _listOfRoles.Add(_homeOwner); 
    }
    
    private void SetupDefaultRequests()
    {
        _postHomeOwnerRequest = new PostHomeOwnerRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl
        };
        
        _putHomeOwnerRequest = new PutHomeOwnerRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl
        };
    }

    private void SetupDefaultUser()
    {
        _user = new User
        {
            Name = _postHomeOwnerRequest.Name,
            Email = _postHomeOwnerRequest.Email,
            Password = _postHomeOwnerRequest.Password,
            Surname = _postHomeOwnerRequest.Surname,
            Photo = _postHomeOwnerRequest.Photo,
            Roles = _listOfRoles
        };
    }
}