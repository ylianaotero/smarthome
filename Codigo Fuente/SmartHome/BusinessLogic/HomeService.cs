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
    IRepository<Domain.Concrete.DeviceUnitService> deviceUnitRepository) : IHomeService
{
    private const string HomeNotFoundMessage = "Home not found";
    private const string DeviceNotFoundMessage = "Device not found";
    private const string UserNotFoundMessage = "Member not found";
    private const string UserIsNotHomeOwnerMessage = "Member is not a home owner";
    private const string HomeAlreadyExists = "A home with this id already exists";

    public void UpdateHome(long homeId)
    {
        Home home = GetHomeById(homeId);
        
        homeRepository.Update(home);
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
    
    public List<Domain.Concrete.DeviceUnitService> GetDevicesFromHome(long homeId)
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
        homeRepository.Update(home);
    }
    
    public void AddDevicesToHome(long homeId, List<DeviceUnitDTO> homeDevices)
    {
        Home home = GetHomeById(homeId);
        
        List<Domain.Concrete.DeviceUnitService> devices = new List<Domain.Concrete.DeviceUnitService>();
      
        MapDevices(homeDevices, devices);
        
        foreach(var device in devices)
        {
            home.Devices.Add(device);
        }
        
        homeRepository.Update(home);
    }
    
    public void UpdateHomeAlias(long id, string alias)
    {
        Home home = GetHomeById(id);
        
        home.Alias = alias;
        
        homeRepository.Update(home);
    }
    
    private void MapDevices(List<DeviceUnitDTO> homeDevices, List<Domain.Concrete.DeviceUnitService> devices)
    {
        foreach (var device in homeDevices)
        {
            Device deviceEntity = deviceRepository.GetById(device.DeviceId);
            
            if (deviceEntity == null)
            {
                throw new ElementNotFound(DeviceNotFoundMessage);
            }

            Domain.Concrete.DeviceUnitService deviceUnitService = new Domain.Concrete.DeviceUnitService()
            {
                Device = deviceEntity,
                Name = deviceEntity.Name,
                IsConnected = device.IsConnected,
                HardwareId = Guid.NewGuid(),
            };
            
            devices.Add(deviceUnitService);
            
            deviceUnitRepository.Add(deviceUnitService);
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

        Role role = userRoles.Find(r => r is HomeOwner);
        if (role == null)
        {
            throw new CannotAddItem(UserIsNotHomeOwnerMessage);
        }

        return (user, role as HomeOwner);
    }
}