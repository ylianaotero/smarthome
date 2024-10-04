using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class HomeService (IRepository<Home> homeRepository) : IHomeService
{
    private IHomeService _homeServiceImplementation;
    private const string HomeNotFoundMessage = "Home not found";
    private const string MemberAlreadyExistsMessage = "A member with this email already exists on this home";
    private const string HomeAlreadyExists = "A home with this id already exists";
    
    public void CreateHome(Home home)
    {
        try
        {
            GetHomeById(home.Id);
            
            throw new CannotAddItem(HomeAlreadyExists);
        }
        catch (ElementNotFound)
        {
            homeRepository.Add(home);
        }
    }
    
    public List<Home> GetAllHomes()
    {
        return homeRepository.GetAll(null);
    }

    public List<Home> GetHomesByFilter(Func<Home, bool> filter)
    {
        return homeRepository.GetByFilter(filter, null);
    }
    
    public List<Member> GetMembersFromHome(long homeId)
    {
        Home home = GetHomeById(homeId);
        return home.Members;
    }
    
    public List<Device> GetDevicesFromHome(int homeId)
    {
        Home home = GetHomeById(homeId);
        return home.Devices;
    }

    public void AddMemberToHome(int homeId, Member member)
    {
        Home home = GetHomeById(homeId);
        if (home.Members.Any(m => m.User.Email == member.User.Email))
        {
            throw new ElementAlreadyExist(MemberAlreadyExistsMessage);
        }
        home.AddMember(member);
        homeRepository.Update(home);
    }

    public Home GetHomeById(long id)
    {
        Home home = homeRepository.GetById(id);

        if (home == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }

        return home;
    }

    public Home PutDevicesInHome(long homeId, List<Device> homeDevices)
    {
        Home home = GetHomeById(homeId);
        home.Devices = homeDevices;
        homeRepository.Update(home);
        
        return home;
    }
}