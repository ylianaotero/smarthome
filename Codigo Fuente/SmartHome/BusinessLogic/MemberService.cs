using CustomExceptions;
using Domain.Concrete;
using Domain.DTO;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class MemberService : IMemberService
{
    private const string MemberAlreadyExistsMessage = "A member with this email already exists on this home";
    private const string HomeIsFullMessage = "The home is full";
    private const string HomeNotFoundMessage = "Home not found";
    private const string UserDoesNotExistExceptionMessage = "Member not found";
    
    private readonly IRepository<Member> _memberRepository;
    private readonly IUserService _userService;
    private readonly IHomeService _homeService;
    
    public MemberService(IRepository<Member> memberRepository, IUserService userService, IHomeService homeService)
    {
        this._memberRepository = memberRepository;
        this._userService = userService;
        this._homeService = homeService;
    }

    public void AddMemberToHome(long homeId, MemberDTO memberDto)
    {
        Home home = _homeService.GetHomeById(homeId);
        
        if (home.Members.Exists(m => m.User.Email == memberDto.UserEmail))
        {
            throw new ElementAlreadyExist(MemberAlreadyExistsMessage);
        }
        
        if (home.Members.Count >= home.MaximumMembers)
        {
            throw new CannotAddItem(HomeIsFullMessage);
        }
        
        User user = GetBy(u => u.Email == memberDto.UserEmail, PageData.Default);

        Member member = new Member()
        {
            User = user,
            Notifications = new List<Notification>(),
            HasPermissionToAddADevice = memberDto.HasPermissionToAddADevice,
            HasPermissionToListDevices = memberDto.HasPermissionToListDevices,
            ReceivesNotifications = memberDto.ReceivesNotifications
        }; 
        
        _memberRepository.Add(member);
        home.AddMember(member);
        _homeService.UpdateHome(homeId);
    }

    public void UpdateMemberNotificationPermission(long homeId, MemberDTO memberDto)
    {
        Home home = _homeService.GetHomeById(homeId);
        
        Member member = _homeService
            .GetMembersFromHome(home.Id).Find(m => m.User.Email == memberDto.UserEmail);
        
        if (member == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }
        
        member.ReceivesNotifications = memberDto.ReceivesNotifications;
        _memberRepository.Update(member);
    }

    private User GetBy(Func<User, bool> predicate, PageData pageData)
    {
        User user = _userService.GetUsersByFilter(predicate, pageData).FirstOrDefault();
        
        if (user == null)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }
        
        return user;
    }
}