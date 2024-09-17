using BusinessLogic.IServices;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Out;

namespace TestWebApi;

[TestClass]
public class UserControllerTest
{
   private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private const string Name =  "John";
    private const string Email1 = "john.doe@example.com";
    private const string Email2 = "john.lopez@example.com";
    private const string Password = "Securepassword1@";
    private const string Surname = "Doe";

    private List<Role> _listOfRoles;
    
    private Mock<IUserService> _userServiceMock;
    private UserController _userController;
    
    [TestInitialize]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);

        _userController = new UserController(_userServiceMock.Object);

        _listOfRoles = new List<Role>();
        
    }

    [TestMethod]
    public void CreateUserValidRequest()
    {

        var user1 = new User
        {
            Name = Name,
            Email = Email1,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl,
            Roles = _listOfRoles
        };
        
        var user2 = new User
        {
            Name = Name,
            Email = Email2,
            Password = Password,
            Surname = Surname,
            Photo = ProfilePictureUrl,
            Roles = _listOfRoles
        };

        string fullName = Name + " " + Surname; 

        List<User> listOfUsers = new List<User> { user1, user2 };
        _userServiceMock.Setup(service => service.GetAllUsers()).Returns(listOfUsers); 
        
        var token = "testToken";

        var result = _userController.GetUsers(token) as OkObjectResult;
        List<UserResponse> userResponse = result.Value as List<UserResponse>;

        _userServiceMock.Verify(service => service.GetAllUsers(), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsNotNull(userResponse);
        foreach (var user in userResponse)
        {
            Assert.AreEqual(Name, user.Name);
            Assert.AreEqual(Surname, user.Surname);
            Assert.AreEqual( fullName, user.FullName);
            Assert.AreEqual(DateTime.Today.Date, user.CreatedAt.Date);
            Assert.AreEqual(0, user.Roles.Count);
        }
    }
    
}