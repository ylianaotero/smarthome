using BusinessLogic.Exceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

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
    
    public List<Member> GetMembersFromHome(int homeId)
    {
        var home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new ElementNotFound("Home not found");
        }
        return home.Members;
    }
    
    public List<Device> GetDevicesFromHome(int homeId)
    {
        var home = homeRepository.GetById(homeId);
        if (home == null)
        {
            throw new ElementNotFound("Home not found");
        }
        return home.Devices;
    }

    public void AddMemberToHome(int homeId, Member member)
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
}