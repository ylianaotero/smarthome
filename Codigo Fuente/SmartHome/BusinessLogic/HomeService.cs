using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class HomeService : IHomeService
{
    private const string HomeNotFoundMessage = "Home not found";
    private const string UserIsNotHomeOwnerMessage = "Member is not a home owner";
    private const string HomeAlreadyExists = "A home with this id already exists";
    
    private readonly IRepository<Home> _homeRepository;
    private readonly IUserService _userService;
    
    public HomeService(IRepository<Home> homeRepository, IUserService userService)
    {
        this._homeRepository = homeRepository;
        this._userService = userService;
    }

    public void UpdateHome(long homeId)
    {
        Home home = GetHomeById(homeId);
        
        _homeRepository.Update(home);
    }
    
    public void CreateHome(Home home)
    {
        try
        {
            GetHomeById(home.Id);
            
            throw new CannotAddItem(HomeAlreadyExists);
        }
        catch (ElementNotFound)
        {
            _homeRepository.Add(home);
        }
    }
    
    public Home GetHomeById(long id)
    {
        Home home = _homeRepository.GetById(id);

        if (home == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }

        return home;
    }

    public List<Home> GetHomesByFilter(Func<Home, bool> filter)
    {
        return _homeRepository.GetByFilter(filter, null);
    }
    
    public List<Member> GetMembersFromHome(long homeId)
    {
        Home home = GetHomeById(homeId);
        
        return home.Members;
    }
    
    public List<DeviceUnit>? GetDevicesFromHomeByFilter(long homeId, Func<DeviceUnit, bool> filter)
    {
        List<DeviceUnit> devices = GetDevicesFromHome(homeId);

        return devices.Where(filter) as List<DeviceUnit>;
    }
    
    public List<DeviceUnit> GetDevicesFromHome(long homeId)
    {
        Home home = GetHomeById(homeId);
        
        return home.Devices;
    }
    
    public List<Room> GetRoomsFromHome(long homeId)
    {
        Home home = GetHomeById(homeId);
        
        return home.Rooms;
    }
    
    public Home AddOwnerToHome(long userId, Home home)
    {
        (User user, HomeOwner role) = GetHomeOwner(userId);
        
        role.Homes.Add(home);
        home.Owner = user;

        return home;
    }
    
    public void AddRoomToHome(long homeId, Room room)
    {
        Home home = GetHomeById(homeId);
        
        home.AddRoom(room);
        _homeRepository.Update(home);
    }
    public void UpdateHomeAlias(long id, string alias)
    {
        Home home = GetHomeById(id);
        
        home.Alias = alias;
        
        _homeRepository.Update(home);
    }
    
    private (User, HomeOwner) GetHomeOwner(long userId)
    {
        User user = _userService.GetUserById(userId);
         
        List<Role> userRoles = user.Roles;
        if (userRoles == null || userRoles.Count == 0)
        {
            throw new CannotAddItem(UserIsNotHomeOwnerMessage);
        }

        Role role = userRoles.Find(r => r is HomeOwner);
        if (role == null)
        {
            throw new CannotAddItem(UserIsNotHomeOwnerMessage);
        }

        return (user, role as HomeOwner);
    }
}