using BusinessLogic.IServices;
using Domain;
using IDataAccess;

namespace BusinessLogic.Services;

public class HomeService (IRepository<Home> homeRepository) : IHomeService
{
    private IHomeService _homeServiceImplementation;

    public void CreateHome(Home home)
    {
        homeRepository.Add(home);
    }
    
    public List<Home> GetAllHomes()
    {
        return homeRepository.GetAll();
    }
    
    public List<Member> GetMembersByHomeId(int homeId)
    {
        var home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new Exception("Home not found");
        }
        return home.Members;
    }
    
    public List<Device> GetDevicesByHomeId(int homeId)
    {
        var home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new Exception("Home not found");
        }
        return home.Devices;
    }

    public void AddMemberToHome(int homeId, Member member)
    {
        var home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new Exception("Home not found");
        }

        home.AddMember(member);
        
        homeRepository.Update(home);
    }
}