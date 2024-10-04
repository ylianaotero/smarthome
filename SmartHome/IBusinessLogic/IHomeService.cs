using Domain;

namespace IBusinessLogic;

public interface IHomeService
{
    void CreateHome(Home home);
    List<Home> GetAllHomes();
    List<Home> GetHomesByFilter(Func<Home, bool> filter);
    List<DeviceUnit> GetDevicesFromHome(int homeId);
    List<Member> GetMembersFromHome(long homeId);
    void AddMemberToHome(long homeId, Member member);
    void ChangePermission(MemberDTO memberDto, long homeId);
    Home AddOwnerToHome(long userId, Home home);
    Home GetHomeById(long id);
    void PutDevicesInHome(long homeId, List<DeviceUnitDTO> devices);
}