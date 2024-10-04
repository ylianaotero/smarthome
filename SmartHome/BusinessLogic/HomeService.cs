using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class HomeService (IRepository<Home> homeRepository) : IHomeService
{
    private IHomeService _homeServiceImplementation;
    private const string HomeNotFoundMessage = "Home not found";


    public void CreateHome(Home home)
    {
        homeRepository.Add(home);
    }
    
    public List<Home> GetAllHomes()
    {
        return homeRepository.GetAll(null);
    }

    public List<Home> GetHomesByFilter(Func<Home, bool> filter)
    {
        return homeRepository.GetByFilter(filter, null);
    }
    
    public List<User> GetMembersFromHome(long homeId)
    {
        var home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new ElementNotFound("Home not found");
        }
        return home.Members;
    }
    
    public List<DeviceUnit> GetDevicesFromHome(int homeId)
    {
        var home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new ElementNotFound("Home not found");
        }
        return home.Devices;
    }

    public void AddMemberToHome(int homeId, User member)
    {
        var home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new ElementNotFound("Home not found");
        }
        if (home.Members.Any(m => m.Email == member.Email))
        {
            throw new ElementAlreadyExist("A member with this email already exists on this home");
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

    public void PutDevicesInHome(long homeId, List<DeviceUnitDTO> homeDevices)
    {
        Home home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }
        
        List<DeviceUnit> devices = new List<DeviceUnit>();
        int cont = 0;
        foreach (var device in homeDevices)
        {
            if (cont == 0)
            {
                cont++;
                devices.Add(new DeviceUnit
                {
                    Device = new WindowSensor()
                    {
                        Id = device.DeviceId
                    },
                    IsConnected = device.IsConnected
                });
            }
            else
            {
                devices.Add(new DeviceUnit
                {
                    Device = new SecurityCamera()
                    {
                        Id = device.DeviceId
                    },
                    IsConnected = device.IsConnected
                });
            }
        }
        
        home.Devices = devices;
        
        homeRepository.Update(home);
    }
}