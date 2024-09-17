using BusinessLogic.IServices;
using BusinessLogic.Services;
using Domain;
using Domain.Exceptions.GeneralExceptions;
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

        [TestInitialize]
        public void Init()
        {
            _user = new User();
            _user.Email = ValidEmail;
            _user.Password = ValidPassword; 
            
            _mockUserRepository = new Mock<IRepository<User>>(MockBehavior.Strict);
            _mockSessionRepository = new Mock<IRepository<Session>>(MockBehavior.Strict);
        }

        [TestMethod]
        public void LogIn()
        {
            Session session = new Session();
            session.User = _user; 

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
            Session session = new Session();
            session.User = _user; 

            _mockUserRepository.Setup(logic => logic.GetByFilter(It.IsAny<Func<User, bool>>())).Returns(new List<User>());
            
            var sessionService = new SessionService(_mockUserRepository.Object,_mockSessionRepository.Object);

            var result = sessionService.LogIn(ValidEmail, ValidPassword);

            _mockUserRepository.VerifyAll();
            _mockSessionRepository.VerifyAll();
            
            
        }
}