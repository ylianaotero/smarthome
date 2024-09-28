using Domain;

namespace IBusinessLogic;

public interface IHomeService
{
    void CreateHome(Home home);
    List<Home> GetAllHomes();
    List<Home> GetHomesByFilter(Func<Home, bool> filter);
    List<Member> GetMembersFromHome(long homeId);
    List<Device> GetDevicesFromHome(int homeId);
    void AddMemberToHome(int homeId,Member member);
    Home GetHomeById(long id);
    Home PutDevicesInHome(long homeId, List<Device> devices);
}