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
    void AddRoomToHome(long homeId, Room room);
    void UpdateMemberNotificationPermission(MemberDTO memberDto, long homeId);
    void AddDevicesToHome(long homeId, List<DeviceUnitDTO> homeDevices);
    void UpdateDeviceConnectionStatus(long id, DeviceUnit deviceUnit);
    void UpdateHomeAlias(long id, string alias);
    void UpdateDeviceCustomName(long id, DeviceUnit device, Guid deviceId);
    void UpdateDeviceRoom(long id, DeviceUnit device, Room room);
}