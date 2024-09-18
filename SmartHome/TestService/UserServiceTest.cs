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

}