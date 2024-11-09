using Domain.Concrete;

namespace IBusinessLogic;

public interface IHomeService
{
    void CreateHome(Home home);
    Home GetHomeById(long id);
    List<Home> GetHomesByFilter(Func<Home, bool> filter);
    List<DeviceUnit> GetDevicesFromHome(long homeId);
    List<Member> GetMembersFromHome(long homeId);
    List<Room> GetRoomsFromHome(long homeId);
    Home AddOwnerToHome(long userId, Home home);
    void AddRoomToHome(long homeId, Room room);
    void UpdateHomeAlias(long id, string alias);
    void UpdateHome(long homeId);
}