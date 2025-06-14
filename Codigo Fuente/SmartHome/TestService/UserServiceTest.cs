using BusinessLogic;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
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
    private const string InvalidRole = "InvalidTole";
    
    private Mock<IRepository<User>> _mockUserRepository;
    private IUserService _userService;

    private User _user;
    private HomeOwner _homeOwner;
    private Company _company;
    private CompanyOwner _companyOwner;
    private Administrator _administrator;
    private List<User> _listOfUsers; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _userService = new UserService(_mockUserRepository.Object);
        
        _user = new User
        {
            Name = NewName,
            Email = NewEmail,
            Password = NewPassword,
            Surname = NewSurname,
            Id = 1
        };
        
        _company = new Company();
        _homeOwner = new HomeOwner();
        _listOfUsers = new List<User>();
        _companyOwner = new CompanyOwner();
        _administrator = new Administrator();
    }
    
    [TestMethod]
    public void CreateUser()
    {
        _mockUserRepository
            .Setup(repo => repo
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<User>());
        
        _mockUserRepository
            .Setup(repo => repo
                .GetAll(It.IsAny<PageData>())).Returns(new List<User>(){_user});
        
        _userService.CreateUser(_user);
        
        _mockUserRepository
            .Verify(repo => repo
                .Add(It.Is<User>(u => u == _user)), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void CreateUserThatAlreadyExists()
    {
        _listOfUsers.Add(_user);
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);
        
        _userService.CreateUser(_user);
        
        _userService.CreateUser(_user);
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void GetUsersWithFilter()
    {
        _listOfUsers.Add(_user);
        Func<User, bool> filter = u => u.Email == NewEmail && 
                                       u.Name == NewName && 
                                       u.Surname == NewSurname 
                                       && u.Password == NewPassword;
        
        _mockUserRepository
            .Setup(v => v.GetByFilter(filter, It.IsAny<PageData>())).Returns(_listOfUsers);
        
        List<User> responseList = _userService.GetUsersByFilter(filter, PageData.Default);
        
        _mockUserRepository.VerifyAll();
        
        Assert.AreEqual(_listOfUsers, responseList);

    }
    
    [TestMethod]
    public void UserIsAdmin()
    {
       _user.Roles = new List<Role>{_administrator,_homeOwner};
        
        _listOfUsers.Add(_user);
        
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);
        
        bool response = _userService.IsAdmin(NewEmail);
        
        _mockUserRepository.VerifyAll();
        
        Assert.IsTrue(response);
        
    }
    
    [TestMethod]
    public void UserIsIncompleteCompanyOwner()
    {
        _user.Roles = new List<Role>{_administrator,_companyOwner};
        
        _listOfUsers.Add(_user);
        
        _mockUserRepository
            .Setup(v => v
                .GetById(_user.Id))
            .Returns(_user);
        
        bool response = _userService.CompanyOwnerIsComplete(_user.Id);
        
        _mockUserRepository.VerifyAll();
        
        Assert.IsFalse(response);
        
    }
    
    [TestMethod]
    public void UserIsCompleteCompanyOwner()
    {
        _companyOwner.HasACompleteCompany = true; 
        _user.Roles = new List<Role>{_administrator,_companyOwner};
        
        _listOfUsers.Add(_user);
        
        _mockUserRepository
            .Setup(v => v
                .GetById(_user.Id))
            .Returns(_user);
        
        bool response = _userService.CompanyOwnerIsComplete(_user.Id);
        
        _mockUserRepository.VerifyAll();
        
        Assert.IsTrue(response);
        
    }
    
    [TestMethod]
    public void TestAddCompanyToOwner()
    {
        _companyOwner.HasACompleteCompany = false; 
        _user.Roles = new List<Role>{_administrator,_companyOwner};
        
        _listOfUsers.Add(_user);
        
        _mockUserRepository
            .Setup(v => v
                .GetById(_user.Id))
            .Returns(_user);
        
        Company response = _userService.AddOwnerToCompany(_user.Id, _company);
        
        _mockUserRepository.VerifyAll();
        
        Assert.IsTrue(response.Id == _company.Id && response.Owner.Email == _user.Email);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddCompanyToOwnerThatIsComplete()
    {
        _companyOwner.HasACompleteCompany = true; 
        _user.Roles = new List<Role>{_administrator,_companyOwner};
        
        _listOfUsers.Add(_user);
        
        _mockUserRepository
            .Setup(v => v
                .GetById(_user.Id))
            .Returns(_user);
        
        _userService.AddOwnerToCompany(_user.Id, _company);
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddCompanyToNotFoundOwner()
    {
        _listOfUsers.Add(_user);
        
        _mockUserRepository
            .Setup(v => v
                .GetById(_user.Id))
            .Returns((User?)null);
        
        _userService.AddOwnerToCompany(_user.Id, _company);
        
        _mockUserRepository.VerifyAll();
        
    }
    
    [TestMethod]
    public void UserIsNotAdmin()
    {
        _user.Roles = new List<Role>{_homeOwner};
        
        _listOfUsers.Add(_user);
        
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);
        
        bool response = _userService.IsAdmin(NewEmail);
        
        _mockUserRepository.VerifyAll();
        
        Assert.IsFalse(response);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void UserIsAdminUserNotFound()
    {
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);
        
        _userService.IsAdmin(NewEmail);
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void UserWithoutRolesIsNotAdmin()
    {
        _user.Roles = new List<Role>();
        
        _listOfUsers.Add(_user);
        
        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);
        
        bool response = _userService.IsAdmin(NewEmail);
        
        _mockUserRepository.VerifyAll();
        
        Assert.IsFalse(response);
        
    }
    
    [TestMethod]
    public void TestDeleteUserWithOnlyAdministratorRole()
    {
        _user.Roles = new List<Role> { _administrator };
        _listOfUsers.Add(_user);

        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);

        _userService.DeleteUser(_user.Id);

        _mockUserRepository
            .Verify(repo => repo.Delete(It.Is<User>(u => u == _user)), Times.Once);
    }

    [TestMethod]
    public void TestDeleteUserWithTwoRoles()
    {
        _user.Roles = new List<Role> { _administrator, _homeOwner };
        _listOfUsers.Add(_user);

        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);

        _userService.DeleteUser(_user.Id);

        _mockUserRepository
            .Verify(repo => repo.Update(It.Is<User>(u => u.Roles.Count == 1 && !u.Roles.Contains(_administrator))), Times.Once);

        _mockUserRepository
            .Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void DeleteUserReturnsNotFound()
    {
        _mockUserRepository
            .Setup(v => v.GetById(_user.Id))
            .Returns((User?)null);
        _userService.DeleteUser(_user.Id);
        _mockUserRepository.Verify();
    }
    
    [TestMethod]
    public void TestUpdateUser()
    {
        _listOfUsers.Add(_user);

        _mockUserRepository
            .Setup(v => v
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(_listOfUsers);
        
        _userService.UpdateUser(_user.Id, _user);

        _mockUserRepository
            .Verify(repo => repo
                .Update(It.Is<User>(u => u.Id == _user.Id)), Times.Once);
        Assert.AreEqual(_user.Email,_user.Email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestUpdateUserNotFound()
    {
        _mockUserRepository
            .Setup(v => v.GetByFilter(It.IsAny<Func<User, bool>>(), PageData.Default))
            .Returns(_listOfUsers);
        
        _userService.UpdateUser(1, _user);
        
        _mockUserRepository.VerifyAll();
    }

    [TestMethod]
    public void TestAssignRoleToUser()
    {
        _mockUserRepository.Setup(v => v.GetById(_user.Id)).Returns(_user);
        
        User user = _userService.AssignRoleToUser(_user.Id, _homeOwner.Kind);
        
        _mockUserRepository.VerifyAll();
        
        Assert.IsTrue(user.Roles.Exists(r => r.Kind == _homeOwner.Kind));
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestAssignRoleThatIsNotAllowedForExistingUsers()
    {
        _mockUserRepository.Setup(v => v.GetById(_user.Id)).Returns(_user);
        
        _userService.AssignRoleToUser(_user.Id, _administrator.Kind);
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestAssignInvalidRoleToUser()
    {
        _mockUserRepository.Setup(v => v.GetById(_user.Id)).Returns(_user);
        
        _userService.AssignRoleToUser(_user.Id, InvalidRole);
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestAssignRoleToUserThatAlreadyHasIt()
    {
        _user.Roles.Add(_homeOwner);
        
        _mockUserRepository.Setup(v => v.GetById(_user.Id)).Returns(_user);
        
        _userService.AssignRoleToUser(_user.Id, _homeOwner.Kind);
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void TestGetUserById()
    {
        _mockUserRepository.Setup(v => v.GetById(_user.Id)).Returns(_user);
        
        User response = _userService.GetUserById(_user.Id);
        
        _mockUserRepository.VerifyAll();
        
        Assert.AreEqual(_user, response);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestGetUserByIdNotFound()
    {
        _mockUserRepository.Setup(v => v.GetById(_user.Id)).Returns((User?)null);
        
        _userService.GetUserById(_user.Id);
        
        _mockUserRepository.VerifyAll();
    }
}