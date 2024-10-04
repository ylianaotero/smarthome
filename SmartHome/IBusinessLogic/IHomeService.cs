using Domain;

namespace IBusinessLogic;

public interface IHomeService
{
    void CreateHome(Home home);
    List<Home> GetAllHomes();
    List<Home> GetHomesByFilter(Func<Home, bool> filter);
    List<User> GetMembersFromHome(long homeId);
    List<Device> GetDevicesFromHome(int homeId);
    void AddMemberToHome(int homeId, User member);
    Home GetHomeById(long id);
    void PutDevicesInHome(long homeId, List<DeviceUnit> devices);
}