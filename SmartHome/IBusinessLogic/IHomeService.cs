using Domain;

namespace IBusinessLogic;

public interface IHomeService
{
    void CreateHome(Home home);
    List<Home> GetAllHomes();
    List<Home> GetHomesByFilter(Func<Home, bool> filter);
    List<DeviceUnit> GetDevicesFromHome(int homeId);
    List<Member> GetMembersFromHome(long homeId);
    
    void ChangePermission(MemberDTO memberDto, long homeId);
    void AddMemberToHome(long homeId, MemberDTO memberDTO);
    
    Home AddOwnerToHome(long userId, Home home);
    Home GetHomeById(long id);
    void PutDevicesInHome(long homeId, List<DeviceUnitDTO> devices);
    void UpdateDeviceConnectionStatus(long id, DeviceUnit deviceUnit);
}