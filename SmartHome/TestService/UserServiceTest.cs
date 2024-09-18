using BusinessLogic.IServices;
using BusinessLogic.Services;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class UserServiceTest
{
    private const string NewName = "Juan Pérez";
    private const string NewEmail = "juan.perez@example.com";
    private const string NewEmail2 = "juan.lopez@example.com";
    private const string NewPassword = "contraseñaSegura1@";
    private const string NewSurname = "Pérez";
    
    private Mock<IRepository<User>> _mockUserRepository;
    private IUserService _userService;
    
    [TestInitialize]
    public void Setup()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
    }
    
    [TestMethod]
    public void CreateUser()
    {
        var user = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname
        };
        
        List<User> listOfUsers = new List<User>();
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        _userService.CreateUser(user);
        
        _mockUserRepository.Verify(repo => repo.Add(It.Is<User>(u => u == user)), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void CreateUserThatAlreadyExists()
    {
        var user = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname
        };
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(user);
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);

        _userService.CreateUser(user);
        
        _userService.CreateUser(user);
    }
    
    [TestMethod]
    public void GetAllUsers()
    {
        var user1 = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname
        };
        
        var user2 = new User
        {
            Name = NewName,
            Email = NewEmail2,
            Password = NewPassword,
            Surname = NewSurname
        };
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(user1);
        listOfUsers.Add(user2);
        
        _mockUserRepository
            .Setup(v => v.GetAll()).Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        List<User> responseList = _userService.GetAllUsers();
        
        Assert.AreEqual(listOfUsers.Count, responseList.Count );
        
        foreach (var user in listOfUsers)
        {
            Assert.IsTrue(responseList.Contains(user), $"Expected user list to contain {user}");
        }
    }
    
    
    [TestMethod]
    public void UserIsAdmin()
    {
        Administrator admin = new Administrator(); 
        var newUser = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname,
            Roles = new List<Role>{admin}
        };
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(newUser);
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        
        bool response = _userService.IsAdmin(NewEmail);
        
        Assert.IsTrue(response);
        
    }

}