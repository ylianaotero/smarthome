using Domain.Concrete;
using Domain.DTO;

namespace IBusinessLogic;

public interface IHomeService
{
    void CreateHome(Home home);
    Home GetHomeById(long id);
    List<Home> GetHomesByFilter(Func<Home, bool> filter);
    List<DeviceUnit> GetDevicesFromHome(int homeId);
    List<Member> GetMembersFromHome(long homeId);
    Home AddOwnerToHome(long userId, Home home);
    void AddMemberToHome(long homeId, MemberDTO memberDTO);
    void UpdateMemberNotificationPermission(MemberDTO memberDto, long homeId);
    void AddDevicesToHome(long homeId, List<DeviceUnitDTO> devices);
    void UpdateDeviceConnectionStatus(long id, DeviceUnit deviceUnit);
    void UpdateHomeAlias(long isAny, string s);
    void UpdateDeviceCustomName(long id, DeviceUnit device);
}