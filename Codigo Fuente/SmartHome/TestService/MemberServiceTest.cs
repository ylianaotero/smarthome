using BusinessLogic;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using Domain.DTO;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class MemberServiceTest
{
    private Mock<IRepository<Member>> _mockMemberRepository;
    private Mock<IUserService> _mockUserService;
    private Mock<IHomeService> _mockHomeService; 
    
    private MemberService _memberService;
    private Home _defaultHome;
    private User _user1;
    private User _user2;
    private User _defaultOwner;
    private MemberDTO _memberDTO2;
    private MemberDTO _memberDTO1; 
    private Member _member1; 
    
    private const string Alias = "My Home";
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    private const string NewEmail = "juan.perez@example.com";
    private const string NewEmail2 = "juan.lopez@example.com";
    private const int MaxMembers = 10;
    private const int Id = 999;
    private const string HomeNotFoundMessage = "Home not found";
    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateMocks();
        SetupDefaultObjects();
    }
    
    [TestMethod]
    public void TestAddMemberToHome()
    {
        _mockHomeService.Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        _mockUserService
            .Setup(v => v
                .GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<User>() {_user1});
        
        _memberService.AddMemberToHome(_defaultHome.Id, _memberDTO1);
        
        _mockHomeService.Verify(m => m.UpdateHome(_defaultHome.Id), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestCannotAddMemberToHomeBecauseItIsAlreadyExist()
    {
        _defaultHome.AddMember(_member1);
    
        _mockHomeService.Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);

        _memberService.AddMemberToHome(_defaultHome.Id, _memberDTO1);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotAddMemberToHomeBecauseItIsNotFound()
    {
        int nonExistentHomeId = Id;
    
        _mockHomeService
            .Setup(m => m
                .GetHomeById(nonExistentHomeId))
            .Throws(new ElementNotFound(HomeNotFoundMessage));
        
        _memberService.AddMemberToHome(nonExistentHomeId, _memberDTO1);
    }
    
    [TestMethod]
    public void TestChangePermission()
    {
        MemberDTO memberDto = new MemberDTO()
        {
            UserEmail = _user1.Email,
            ReceivesNotifications = true
        }; 
        
        _defaultHome.AddMember(_member1);
        
        _mockHomeService
            .Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        _mockHomeService
            .Setup(m => m.GetMembersFromHome(_defaultHome.Id))
            .Returns(new List<Member>() {_member1});
        
        _memberService.UpdateMemberNotificationPermission(_defaultHome.Id, memberDto);
        
        Assert.AreEqual(memberDto.ReceivesNotifications, 
            _defaultHome.FindMember(memberDto.UserEmail).ReceivesNotifications);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotChangePermissionBecauseHomeNotFound()
    {
        MemberDTO memberDto = new MemberDTO()
        {
            UserEmail = _user1.Email,
            ReceivesNotifications = true
        }; 
        
        _defaultHome.AddMember(_member1);
        _mockHomeService
            .Setup(m => m
                .GetHomeById(_defaultHome.Id))
            .Throws(new ElementNotFound(HomeNotFoundMessage));
        
        _memberService.UpdateMemberNotificationPermission(_defaultHome.Id, memberDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotChangePermissionBecauseMemberNotFound()
    {
        MemberDTO memberDto = new MemberDTO()
        {
            UserEmail = _user1.Email,
            ReceivesNotifications = true
        }; 
        
        _mockHomeService
            .Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        _mockHomeService
            .Setup(m => m.GetMembersFromHome(_defaultHome.Id))
            .Returns(new List<Member>());
        
        _memberService.UpdateMemberNotificationPermission(_defaultHome.Id, memberDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestAddMemberToHomeFailsIfMaxMembersHasBeenReached()
    {
        _mockHomeService.Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        
        _mockUserService
            .Setup(v => v
                .GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<User>() {_user1, _user2});
        
        _memberService.AddMemberToHome(_defaultHome.Id, _memberDTO1);
        _memberService.AddMemberToHome(_defaultHome.Id, _memberDTO2);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddMemberToHomeFailsIfUserNotFound()
    {
        _mockHomeService.Setup(m => m.GetHomeById(_defaultHome.Id)).Returns(_defaultHome);
        
        _mockUserService
            .Setup(v => v
                .GetUsersByFilter(It.IsAny<Func<User, bool>>(), It.IsAny<PageData>()))
            .Returns(new List<User>());
        
        _memberService.AddMemberToHome(_defaultHome.Id, _memberDTO1);
    }
    
    private void SetupDefaultObjects()
    {
        _memberService = new MemberService
            (_mockMemberRepository.Object, _mockUserService.Object, _mockHomeService.Object);
        
        SetUpDefaultHome();
        SetUpDefaultMembers();
    }
    
    private void SetUpDefaultHome()
    {
        _defaultOwner = new User()
        {
            Email = NewEmail,
            Id = 1,
            Roles = new List<Role>()
            {
                new HomeOwner(),
            }
        };
        
        _defaultHome = new Home()
        {
            Id = 1,
            Owner = _defaultOwner,
            Alias = Alias,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = MaxMembers
        };
    }

    private void SetUpDefaultMembers()
    {
        _user1 = new User() {Email = NewEmail};
        _user2 = new User() {Email = NewEmail2};
        _member1 = new Member(_user1);
        _memberDTO1 = new MemberDTO() { UserEmail = NewEmail };
        _memberDTO2 = new MemberDTO() { UserEmail = NewEmail2 }; 
    }

    private void CreateMocks()
    {
        _mockMemberRepository = new Mock<IRepository<Member>>();
        _mockUserService = new Mock<IUserService>();
        _mockHomeService = new Mock<IHomeService>();
    }
}