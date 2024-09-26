using BusinessLogic;
using CustomExceptions;
using Domain;
using IBusinessLogic;
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

    private User _user;
    private List<User> _listOfUsers; 
    
    
    [TestInitialize]
    public void Setup()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        
        _user = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname
        };
        
        _listOfUsers = new List<User>();
        _listOfUsers.Add(_user);
    }
    
    [TestMethod]
    public void CreateUser()
    {
        _mockUserRepository
            .Setup(repo => repo.GetByFilter(It.IsAny<Func<User, bool>>()))
            .Returns(new List<User>());
        
        
        _userService = new UserService(_mockUserRepository.Object);
        
        _userService.CreateUser(_user);
        
        _mockUserRepository.Verify(repo => repo.Add(It.Is<User>(u => u == _user)), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void CreateUserThatAlreadyExists()
    {
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(_listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);

        _userService.CreateUser(_user);
        
        _userService.CreateUser(_user);
        
        _mockUserRepository.Verify();
    }
    
    [TestMethod]
    public void GetAllUsers()
    {
        var user2 = new User
        {
            Name = NewName,
            Email = NewEmail2,
            Password = NewPassword,
            Surname = NewSurname
        };
        
        _listOfUsers.Add(user2);
        
        _mockUserRepository
            .Setup(v => v.GetAll()).Returns(_listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        List<User> responseList = _userService.GetAllUsers();
        
        _mockUserRepository.Verify();
        
        Assert.IsTrue(
            _listOfUsers.Count == responseList.Count && 
            _listOfUsers.All(user => responseList.Contains(user))
        );

    }
    
    
    [TestMethod]
    public void UserIsAdmin()
    {
        HomeOwner homeOwner = new HomeOwner(); 
        Administrator admin = new Administrator(); 
        var newUser = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname,
            Roles = new List<Role>{admin,homeOwner}
        };
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(newUser);
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        
        bool response = _userService.IsAdmin(NewEmail);
        
        _mockUserRepository.Verify();
        
        Assert.IsTrue(response);
        
    }
    
    [TestMethod]
    public void UserIsNotAdmin()
    {
        HomeOwner homeOwner = new HomeOwner(); 
        var newUser = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname,
            Roles = new List<Role>{homeOwner}
        };
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(newUser);
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        
        bool response = _userService.IsAdmin(NewEmail);
        
        _mockUserRepository.Verify();
        
        Assert.IsFalse(response);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void UserIsAdminUserNotFound()
    {
        List<User> listOfUsers = new List<User>();
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        _userService.IsAdmin(NewEmail);
        
        _mockUserRepository.Verify();
        
    }
    
    [TestMethod]
    public void UserWithoutRolesIsNotAdmin()
    {
        var newUser = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname,
            Roles = new List<Role>()
        };
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(newUser);
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        
        bool response = _userService.IsAdmin(NewEmail);
        
        _mockUserRepository.Verify();
        
        Assert.IsFalse(response);
        
    }
    
    [TestMethod]
    public void TestDeleteUser()
    {
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(_listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        _userService.DeleteUser(1);
        
        _mockUserRepository.Verify(repo => repo.Delete(It.Is<User>(u => u == _user)), Times.Once);
    }


}