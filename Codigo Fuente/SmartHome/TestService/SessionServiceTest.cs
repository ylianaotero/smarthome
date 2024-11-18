using BusinessLogic;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class SessionServiceTest
{
    private IUserService _userService;
    private ISessionService _sessionService;
    private Mock<IRepository<User>> _mockUserRepository;
    private Mock<IRepository<Session>> _mockSessionRepository;
    
    private User _user;
    private Home _home;
    private Session _session; 
    
    private const string Permission = "Administrator";
    private const string ValidPassword = "juanLop1@";
    private const string ValidEmail = "juanlopez@gmail.com";
    
    [TestInitialize]
    public void TestInitialize()
    {
        _user = new User();
        _user.Email = ValidEmail;
        _user.Password = ValidPassword; 
        
        _session = new Session();
        _session.User = _user;
        
        _home = new Home()
        {
            Owner = _user
        };
        
        _mockUserRepository = new Mock<IRepository<User>>(MockBehavior.Strict);
        _mockSessionRepository = new Mock<IRepository<Session>>(MockBehavior.Strict);
        
        _sessionService = new SessionService(_mockUserRepository.Object,_mockSessionRepository.Object);
    }

    [TestMethod]
    public void LogIn()
    {
        _mockUserRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<User> { _user });


        _mockSessionRepository
            .Setup(repo => repo.Add(It.IsAny<Session>()))
            .Verifiable();
        
        Session result = _sessionService.LogIn(ValidEmail, ValidPassword);

        _mockUserRepository.VerifyAll();
        _mockSessionRepository.VerifyAll();
        
        _mockSessionRepository
            .Verify(repo => repo
                .Add(It.Is<Session>(u => u == result)), Times.Once);
        Assert.AreEqual(result.User, _user);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TryToLogInWithUserThatDoesNotExist()
    {
        _mockUserRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<User>());
        
        _sessionService.LogIn(ValidEmail, ValidPassword);

        _mockUserRepository.VerifyAll();
        _mockSessionRepository.VerifyAll();
    }
    
    [TestMethod]
    public void LogOut()
    {
        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session> { _session });

        _mockSessionRepository
            .Setup(repo => repo.Delete(It.IsAny<Session>()))
            .Verifiable();
        
        _sessionService.LogOut(_session.Id);

        _mockUserRepository.VerifyAll();
        _mockSessionRepository.VerifyAll();
        
        _mockSessionRepository
            .Verify(repo => repo.Delete(It.IsAny<Session>()), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void InvalidLogOut()
    {

        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session>());
        
        _sessionService.LogOut(_session.Id);

        _mockUserRepository.VerifyAll();
        _mockSessionRepository.VerifyAll();
    }
    
    [TestMethod]
    public void GetUserOfTheSession()
    {
        _session.Id = new Guid(); 

        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session> { _session });
        
        User response = _sessionService.GetUser(_session.Id);

        _mockUserRepository.VerifyAll();
        _mockSessionRepository.VerifyAll();
        
        Assert.AreEqual(response,_user);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void GetUserOfANotExistingSession()
    {
        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session>());
        
        _sessionService.GetUser(new Guid());

        _mockUserRepository.VerifyAll();
        _mockSessionRepository.VerifyAll();
    }
    
    [TestMethod]
    public void UserHasPermissions()
    {
        _session.Id = new Guid(); 
        _user.Roles = new List<Role> { new Administrator() };

        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session> { _session });
        
        bool response = _sessionService.UserHasCorrectRole(_session.Id, Permission);

        _mockUserRepository.VerifyAll();
        _mockSessionRepository.VerifyAll();
        
        Assert.IsTrue(response);
    }

    [TestMethod]
    public void AuthorizationIsValid()
    {
        _session.Id = new Guid(); 

        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session> { _session });
        
        bool response = _sessionService.AuthorizationIsValid(_session.Id);

        _mockUserRepository.VerifyAll();
        _mockSessionRepository.VerifyAll();
        
        Assert.IsTrue(response);
    }
    
    [TestMethod]
    public void UserCanListDevicesInHomeWhenUserIsOwner()
    {
        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session> { _session });
        
         bool response = _sessionService.UserCanListDevicesInHome(Guid.NewGuid(), _home);

        Assert.IsTrue(response);
    }
    
    [TestMethod]
    public void UserCanAddDevicesInHomeWhenUserIsOwner()
    {
        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session> { _session });
        
        bool response = _sessionService.UserCanAddDevicesInHome(Guid.NewGuid(), _home);

        Assert.IsTrue(response);
    }
    
    [TestMethod]
    public void UserCanListDevicesInHomeWhenUserIsPrivilegedMember()
    {
        Member member = new Member()
        {
            User = _user,
            HasPermissionToListDevices = true,
        };
        _home.Members.Add(member);
        
        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session> { _session });
        
        bool response = _sessionService.UserCanListDevicesInHome(Guid.NewGuid(), _home);

        Assert.IsTrue(response);
    }

    [TestMethod]
    public void UserCanAddDevicesInHomeWhenUserIsPrivilegedMember()
    {
        Member member = new Member()
        {
            User = _user,
            HasPermissionToAddADevice = true,
        };
        _home.Members.Add(member);
        
        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session> { _session });
        
        bool response = _sessionService.UserCanAddDevicesInHome(Guid.NewGuid(), _home);

        Assert.IsTrue(response);
    }
    
    
    [TestMethod]
    public void TestGetUserId()
    {
        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session> { _session });

        _sessionService = new SessionService(_mockUserRepository.Object, _mockSessionRepository.Object); 
        
        long response = _sessionService.GetUserId(new Guid());

        Assert.AreEqual(_session.User.Id, response);
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(CannotFindItemInList))]
    public void TestGetUserIdThrowsException()
    {
        _mockSessionRepository
            .Setup(logic => logic
                .GetByFilter(It.IsAny<Func<Session, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<Session>());

        _sessionService = new SessionService(_mockUserRepository.Object, _mockSessionRepository.Object); 
        
        _sessionService.GetUserId(new Guid());
    }
}