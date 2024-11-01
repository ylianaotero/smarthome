using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using Domain.DTO;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class HomeService (
    IRepository<Home> homeRepository, 
    IRepository<Device> deviceRepository, 
    IRepository<User> userRepository, 
    IRepository<Member> memberRepository,
    IRepository<DeviceUnit> deviceUnitRepository) : IHomeService
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
    
    public Home GetHomeById(long id)
    {
        Home home = homeRepository.GetById(id);

        if (home == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }

        return home;
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
    
    public Home AddOwnerToHome(long userId, Home home)
    {
        (User user, HomeOwner role) = GetHomeOwner(userId);
        
        role.Homes.Add(home);
        home.Owner = user;

        return home;
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
        
        memberRepository.Add(member);
        home.AddMember(member);
        homeRepository.Update(home);
    }
    
    public void UpdateMemberNotificationPermission(MemberDTO memberDto, long homeId)
    {
        List<Member> listOfMembers = GetMembersFromHome(homeId); 
        
        Member member = listOfMembers.FirstOrDefault(m => m.User.Email == memberDto.UserEmail);

        if (member == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }
        member.ReceivesNotifications = memberDto.ReceivesNotifications; 
        memberRepository.Update(member);
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
        
        foreach(var device in devices)
        {
            home.Devices.Add(device);
        }
        
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
    
    public void AddRoomToHome(long homeId, Room room)
    {
        Home home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }
        
        home.Rooms.Add(room);
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
    
    private void MapDevices(List<DeviceUnitDTO> homeDevices, List<DeviceUnit> devices)
    {
        foreach (var device in homeDevices)
        {
            Device deviceEntity = deviceRepository.GetById(device.DeviceId);
            
            if (deviceEntity == null)
            {
                throw new ElementNotFound(DeviceNotFoundMessage);
            }

            DeviceUnit deviceUnit = new DeviceUnit()
            {
                Device = deviceEntity,
                Name = deviceEntity.Name,
                IsConnected = device.IsConnected,
                HardwareId = Guid.NewGuid(),
            };
            
            devices.Add(deviceUnit);
            
            deviceUnitRepository.Add(deviceUnit);
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
    
    public void UpdateHomeAlias(long id, string alias)
    {
        Home home = homeRepository.GetById(id);
        if (home == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }
        
        home.Alias = alias;
        
        homeRepository.Update(home);
    }
    
    public void UpdateDeviceCustomName(long id, DeviceUnit device, Guid deviceId)
    {
        Home home = homeRepository.GetById(id)!;
        if (home == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }

        try
        {
            DeviceUnit deviceUnit = home.Devices.FirstOrDefault(d => d.HardwareId == deviceId);
            
            if (deviceUnit == null)
            {
                throw new ElementNotFound(DeviceNotFoundMessage);
            }
            
            deviceUnit.Name = device.Name;
            homeRepository.Update(home);
        }
        catch (ElementNotFound)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
    }
    
}