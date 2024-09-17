using Domain;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class UserServiceTest
{
    private Mock<IRepository<User>> _mockUserRepository;
    private IUserService _userService;
    
    [TestInitialize]
    public void Setup()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _userService = new UserService(_mockUserRepository.Object);
    }
    
    [TestMethod]
    public void CreateUser_ShouldCallAddOnRepository()
    {
        var user = new User
        {
            Name = "Juan Pérez",
            Email = "juan.perez@example.com",
            Password = "contraseñaSegura1@",
            Surname = "Pérez"
        };
        _userService.CreateUser(user);
        
        _mockUserRepository.Verify(repo => repo.Add(It.Is<User>(u => u == user)), Times.Once);
    }

}