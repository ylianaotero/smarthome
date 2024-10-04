using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class HomeService (
    IRepository<Home> homeRepository, 
    IRepository<Device> deviceRepository, 
    IRepository<User> userRepository) : IHomeService
{
    private IHomeService _homeServiceImplementation;
    private const string HomeNotFoundMessage = "Home not found";
    private const string DeviceNotFoundMessage = "Device not found";
    private const string MemberAlreadyExistsMessage = "A member with this email already exists on this home";
    private const string HomeAlreadyExists = "A home with this id already exists";
    private const string HomeIsFullMessage = "The home is full";

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

    public List<DeviceUnit> GetDevicesFromHome(int homeId)
    {
        Home home = GetHomeById(homeId);
        return home.Devices;
    }

    public void AddMemberToHome(long homeId, Member member)
    {
        Home home = GetHomeById(homeId);
        if (home.Members.Any(m => m.User.Email == member.User.Email))
        {
            throw new ElementAlreadyExist(MemberAlreadyExistsMessage);
        }
        
        if (home.Members.Count >= home.MaximumMembers)
        {
            throw new CannotAddItem(HomeIsFullMessage);
        }
        
        home.AddMember(member);
        homeRepository.Update(home);
    }

    public Home AddOwnerToHome(long userId, Home home)
    {
        User user = GetUserById(userId);
        
        HomeOwner role = user.Roles.FirstOrDefault(r => r is HomeOwner) as HomeOwner;
        
        role.Homes.Add(home);
        home.Owner = user;
        
        userRepository.Update(user);
        
        homeRepository.Update(home);

        return home;
    }
    
    private User GetUserById(long userId)
    {
        User user = userRepository.GetById(userId);
        if (user == null)
        {
            throw new ElementNotFound("User not found");
        }

        return user;
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

    public void PutDevicesInHome(long homeId, List<DeviceUnitDTO> homeDevices)
    {
        Home home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }
        
        List<DeviceUnit> devices = new List<DeviceUnit>();
      
        MapDevices(homeDevices, devices);
        
        home.Devices = devices;
        
        homeRepository.Update(home);
    }
    
    private void MapDevices(List<DeviceUnitDTO> homeDevices, List<DeviceUnit> devices)
    {
        foreach (var device in homeDevices)
        {
            Device deviceEntity = deviceRepository.GetById(device.DeviceId);
            
            if (deviceEntity == null)
            {
                throw new ElementNotFound(DeviceNotFoundMessage);
            }
            
            devices.Add(new DeviceUnit
            {
                Device = deviceEntity,
                IsConnected = device.IsConnected,
                HardwareId = Guid.NewGuid(),
            });
        }
    }
}