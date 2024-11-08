using Domain.Concrete;
using Domain.DTO;

namespace IBusinessLogic;

public interface IHomeService
{
    void CreateHome(Home home);
    Home GetHomeById(long id);
    List<Home> GetHomesByFilter(Func<Home, bool> filter);
    List<DeviceUnitService> GetDevicesFromHome(long homeId);
    List<Member> GetMembersFromHome(long homeId);
    List<Room> GetRoomsFromHome(long homeId);
    Home AddOwnerToHome(long userId, Home home);
    void AddRoomToHome(long homeId, Room room);
    void AddDevicesToHome(long homeId, List<DeviceUnitDTO> homeDevices);
    void UpdateHomeAlias(long id, string alias);
    void UpdateHome(long homeId);
}