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
    private const string HomeNotFoundMessage = "Home not found";
    private const string DeviceNotFoundMessage = "Device not found";
    private const string UserNotFoundMessage = "Member not found";
    private const string UserIsNotHomeOwnerMessage = "Member is not a home owner";
    private const string MemberAlreadyExistsMessage = "A member with this email already exists on this home";
    private const string HomeAlreadyExists = "A home with this id already exists";
    private const string HomeIsFullMessage = "The home is full";
    private const string UserDoesNotExistExceptionMessage = "Member not found";

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
    
    public void ChangePermission(MemberDTO memberDto, long homeId)
    {
        List<Member> listOfMembers = GetMembersFromHome(homeId); 
        
        Member member = listOfMembers.FirstOrDefault(m => m.User.Email == memberDto.UserEmail);

        if (member == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }

        member.ReceivesNotifications = memberDto.ReceivesNotifications; 
    }

    public List<DeviceUnit> GetDevicesFromHome(int homeId)
    {
        Home home = GetHomeById(homeId);
        return home.Devices;
    }

    public void AddMemberToHome(long homeId, MemberDTO memberDTO)
    {
        Home home = GetHomeById(homeId);
        if (home.Members.Any(m => m.User.Email == memberDTO.UserEmail))
        {
            throw new ElementAlreadyExist(MemberAlreadyExistsMessage);
        }
        
        if (home.Members.Count >= home.MaximumMembers)
        {
            throw new CannotAddItem(HomeIsFullMessage);
        }
        
        User user = GetBy(u => u.Email == memberDTO.UserEmail, PageData.Default);

        Member member = new Member()
        {
            User = user,
            Notifications = new List<Notification>(),
            HasPermissionToAddADevice = memberDTO.HasPermissionToAddADevice,
            HasPermissionToListDevices = memberDTO.HasPermissionToListDevices,
            ReceivesNotifications = memberDTO.ReceivesNotifications
        }; 
        
        home.AddMember(member);
        homeRepository.Update(home);
    }
    
    private User GetBy(Func<User, bool> predicate, PageData pageData)
    {
        User user = userRepository.GetByFilter(predicate, pageData).FirstOrDefault(); 
        
        if (user == null)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }

        return user; 
    }

    public Home AddOwnerToHome(long userId, Home home)
    {
        (User user, HomeOwner role) = GetHomeOwner(userId);
        
        role.Homes.Add(home);
        home.Owner = user;
        
        userRepository.Update(user);
        
        homeRepository.Update(home);

        return home;
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

    public void AddDevicesToHome(long homeId, List<DeviceUnitDTO> homeDevices)
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
    
    public void UpdateDeviceConnectionStatus(long id, DeviceUnit deviceUnit)
    {
        Home home = homeRepository.GetById(id);
        if(home == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }
        DeviceUnit device = home.Devices.FirstOrDefault(d => d.HardwareId == deviceUnit.HardwareId);
        
        if (device == null)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
        
        device.IsConnected = deviceUnit.IsConnected;
        
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
    
    private (User, HomeOwner) GetHomeOwner(long userId)
    {
        User user = userRepository.GetById(userId);
        if (user == null)
        {
            throw new ElementNotFound(UserNotFoundMessage);
        }
         
        List<Role> userRoles = user.Roles;
        if (userRoles == null || userRoles.Count == 0)
        {
            throw new CannotAddItem(UserIsNotHomeOwnerMessage);
        }

        Role role = userRoles.FirstOrDefault(r => r is HomeOwner);
        if (role == null)
        {
            throw new CannotAddItem(UserIsNotHomeOwnerMessage);
        }

        return (user, role as HomeOwner);
    }
}