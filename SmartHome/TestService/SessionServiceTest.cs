using BusinessLogic;
using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class SessionServiceTest
{
        private const string ValidEmail = "juanlopez@gmail.com";
        private const string ValidPassword = "juanLop1@";
        
        private User _user;
        
        private Mock<IRepository<User>> _mockUserRepository;
        private Mock<IRepository<Session>> _mockSessionRepository;
        private IUserService _userService;
        private ISessionService _sessionService;

        private Session _session; 

        [TestInitialize]
        public void Init()
        {
            _user = new User();
            _user.Email = ValidEmail;
            _user.Password = ValidPassword; 
            
            _mockUserRepository = new Mock<IRepository<User>>(MockBehavior.Strict);
            _mockSessionRepository = new Mock<IRepository<Session>>(MockBehavior.Strict);
            
            _session = new Session();
            _session.User = _user;
        }

        [TestMethod]
        public void LogIn()
        {
            _mockUserRepository.Setup(logic => logic.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(new List<User> { _user });

            _mockSessionRepository.Setup(repo => repo.Add(It.IsAny<Session>())).Verifiable();
            
            var sessionService = new SessionService(_mockUserRepository.Object,_mockSessionRepository.Object);

            var result = sessionService.LogIn(ValidEmail, ValidPassword);

            _mockUserRepository.VerifyAll();
            _mockSessionRepository.VerifyAll();
            
            _mockSessionRepository.Verify(repo => repo.Add(It.Is<Session>(u => u == result)), Times.Once);
            Assert.AreEqual(result.User, _user);
        }
        
        [TestMethod]
        [ExpectedException(typeof(CannotFindItemInList))]
        public void TryToLogInWithUserThatDoesNotExist()
        {
            _mockUserRepository.Setup(logic => logic.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(new List<User>());
            
            var sessionService = new SessionService(_mockUserRepository.Object,_mockSessionRepository.Object);

            var result = sessionService.LogIn(ValidEmail, ValidPassword);

            _mockUserRepository.VerifyAll();
            _mockSessionRepository.VerifyAll();
        }
        
        [TestMethod]
        public void LogOut()
        {
            _mockSessionRepository.Setup(logic => logic.GetByFilter(It.IsAny<Func<Session, bool>>())).Returns(new List<Session> { _session });

            _mockSessionRepository.Setup(repo => repo.Delete(It.IsAny<Session>())).Verifiable();
            
            var sessionService = new SessionService(_mockUserRepository.Object,_mockSessionRepository.Object);

            sessionService.LogOut(_session.Id);

            _mockUserRepository.VerifyAll();
            _mockSessionRepository.VerifyAll();
            
            _mockSessionRepository.Verify(repo => repo.Delete(It.IsAny<Session>()), Times.Once);
        }
        
        [TestMethod]
        [ExpectedException(typeof(CannotFindItemInList))]
        public void InvalidLogOut()
        {

            _mockSessionRepository.Setup(logic => logic.GetByFilter(It.IsAny<Func<Session, bool>>())).Returns(new List<Session>());
            
            var sessionService = new SessionService(_mockUserRepository.Object,_mockSessionRepository.Object);

            sessionService.LogOut(_session.Id);

            _mockUserRepository.VerifyAll();
            _mockSessionRepository.VerifyAll();
        }
        
        [TestMethod]
        public void GetUserOfTheSession()
        {
            _session.Id = new Guid(); 

            _mockSessionRepository.Setup(logic => logic.GetByFilter(It.IsAny<Func<Session, bool>>())).Returns(new List<Session> { _session });
            
            var sessionService = new SessionService(_mockUserRepository.Object,_mockSessionRepository.Object);

            User response = sessionService.GetUser(_session.Id);

            _mockUserRepository.VerifyAll();
            _mockSessionRepository.VerifyAll();
            
            Assert.AreEqual(response,_user);
        }
        
        [TestMethod]
        [ExpectedException(typeof(CannotFindItemInList))]
        public void GetUserOfANotExistingSession()
        {
            _mockSessionRepository.Setup(logic => logic.GetByFilter(It.IsAny<Func<Session, bool>>())).Returns(new List<Session>());
            
            var sessionService = new SessionService(_mockUserRepository.Object,_mockSessionRepository.Object);

            sessionService.GetUser(new Guid());

            _mockUserRepository.VerifyAll();
            _mockSessionRepository.VerifyAll();
        }
        
        [TestMethod]
        public void UserHasPermissions()
        {
            _session.Id = new Guid(); 
            _user.Roles = new List<Role> { new Administrator() };

            _mockSessionRepository.Setup(logic => logic.GetByFilter(It.IsAny<Func<Session, bool>>())).Returns(new List<Session> { _session });
            
            var sessionService = new SessionService(_mockUserRepository.Object,_mockSessionRepository.Object);

            bool response = sessionService.UserHasPermissions(_session.Id, "Administrator");

            _mockUserRepository.VerifyAll();
            _mockSessionRepository.VerifyAll();
            
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void AuthorizationIsValid()
        {
            _session.Id = new Guid(); 

            _mockSessionRepository.Setup(logic => logic.GetByFilter(It.IsAny<Func<Session, bool>>())).Returns(new List<Session> { _session });
            
            var sessionService = new SessionService(_mockUserRepository.Object,_mockSessionRepository.Object);

            bool response = sessionService.AuthorizationIsValid(_session.Id);

            _mockUserRepository.VerifyAll();
            _mockSessionRepository.VerifyAll();
            
            Assert.IsTrue(response);
        }
        
        
    
}