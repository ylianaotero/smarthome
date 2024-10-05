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
    
    private const string UserDoesNotExistExceptionMessage = "Member not found";
    
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
            .Setup(repo => repo
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<User>());
        
        
        _userService = new UserService(_mockUserRepository.Object);
        
        _userService.CreateUser(_user);
        
        _mockUserRepository
            .Verify(repo => repo
                .Add(It.Is<User>(u => u == _user)), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void CreateUserThatAlreadyExists()
    {
        
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);
        
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
            .Setup(v => v.GetAll(It.IsAny<PageData>())).Returns(_listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        List<User> responseList = _userService.GetAllUsers(PageData.Default);
        
        _mockUserRepository.Verify();
        
        Assert.IsTrue(
            _listOfUsers.Count == responseList.Count && 
            _listOfUsers.All(user => responseList.Contains(user))
        );

    }
    
    [TestMethod]
    public void GetUsersWithFilter()
    {
        List<User> users = new List<User>();
        User user1 = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname
        };
        users.Add(user1);
        Func<User, bool> filter = u => u.Email == NewEmail && 
                                       u.Name == NewName && 
                                       u.Surname == NewSurname 
                                       && u.Password == NewPassword;
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(filter, It.IsAny<PageData>())).Returns(users);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        List<User> responseList = _userService.GetUsersByFilter(filter, PageData.Default);
        
        _mockUserRepository.Verify();
        
        Assert.AreEqual(users, responseList);

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
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        
        bool response = _userService.IsAdmin(NewEmail);
        
        _mockUserRepository.Verify();
        
        Assert.IsTrue(response);
        
    }
    
    [TestMethod]
    public void UserIsIncompleteCompanyOwner()
    {
        CompanyOwner companyOwner = new CompanyOwner(); 
        Administrator admin = new Administrator(); 
        var newUser = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname,
            Roles = new List<Role>{admin,companyOwner}
        };
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(newUser);
        
        _mockUserRepository
            .Setup(v => v
                .GetById(newUser.Id))
            .Returns(newUser);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        bool response = _userService.CompanyOwnerIsComplete(newUser.Id);
        
        _mockUserRepository.Verify();
        
        Assert.IsFalse(response);
        
    }
    
    [TestMethod]
    public void UserIsCompleteCompanyOwner()
    {
        CompanyOwner companyOwner = new CompanyOwner();
        companyOwner.HasACompleteCompany = true; 
        Administrator admin = new Administrator(); 
        var newUser = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname,
            Roles = new List<Role>{admin,companyOwner}
        };
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(newUser);
        
        _mockUserRepository
            .Setup(v => v
                .GetById(newUser.Id))
            .Returns(newUser);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        bool response = _userService.CompanyOwnerIsComplete(newUser.Id);
        
        _mockUserRepository.Verify();
        
        Assert.IsTrue(response);
        
    }
    
    [TestMethod]
    public void TestAddCompanyToOwner()
    {
        CompanyOwner companyOwner = new CompanyOwner();
        companyOwner.HasACompleteCompany = false; 
        Administrator admin = new Administrator(); 
        User newUser = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname,
            Roles = new List<Role>{admin,companyOwner}
        };

        Company company = new Company(); 
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(newUser);
        
        _mockUserRepository
            .Setup(v => v
                .GetById(newUser.Id))
            .Returns(newUser);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        Company response = _userService.AddOwnerToCompany(newUser.Id, company);
        
        _mockUserRepository.Verify();
        
        Assert.IsTrue(response.Id == company.Id && response.Owner.Email == newUser.Email);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddCompanyToOwnerThatIsComplete()
    {
        CompanyOwner companyOwner = new CompanyOwner();
        companyOwner.HasACompleteCompany = true; 
        Administrator admin = new Administrator(); 
        User newUser = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname,
            Roles = new List<Role>{admin,companyOwner}
        };

        Company company = new Company(); 
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(newUser);
        
        _mockUserRepository
            .Setup(v => v
                .GetById(newUser.Id))
            .Returns(newUser);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        _userService.AddOwnerToCompany(newUser.Id, company);
        
        _mockUserRepository.Verify();
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddCompanyToNotFoundOwner()
    {
        Administrator admin = new Administrator(); 
        User newUser = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname
        };

        Company company = new Company(); 
        
        List<User> listOfUsers = new List<User>();
        listOfUsers.Add(newUser);
        
        _mockUserRepository
            .Setup(v => v
                .GetById(newUser.Id))
            .Returns((User?)null);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        _userService.AddOwnerToCompany(newUser.Id, company);
        
        _mockUserRepository.Verify();
        
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
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(listOfUsers);
        
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
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(listOfUsers);
        
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
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        
        bool response = _userService.IsAdmin(NewEmail);
        
        _mockUserRepository.Verify();
        
        Assert.IsFalse(response);
        
    }
    
    [TestMethod]
    public void TestDeleteUser()
    {
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        _userService.DeleteUser(1);
        
        _mockUserRepository
            .Verify(repo => repo
                .Delete(It.Is<User>(u => u == _user)), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestDeleteUserNotFound()
    {
        List<User> listOfUsers = new List<User>();
        
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        _userService.DeleteUser(1);
        
        _mockUserRepository.Verify();
    }
    
    [TestMethod]
    public void TestUpdateUser()
    {
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);
        _userService = new UserService(_mockUserRepository.Object);
        
        User newUser = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname
        };
        
        _userService.UpdateUser(_user.Id, newUser);

        _mockUserRepository
            .Verify(repo => repo
                .Update(It.Is<User>(u => u.Id == _user.Id)), Times.Once);
        Assert.AreEqual(_user.Email,newUser.Email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestUpdateUserNotFound()
    {
        List<User> listOfUsers = new List<User>();
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>(), PageData.Default))
            .Returns(listOfUsers);
        
        _userService = new UserService(_mockUserRepository.Object);
        
        _userService.UpdateUser(1, _user);
        
        _mockUserRepository.Verify();
    }

}