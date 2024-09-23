using Domain;

namespace IBusinessLogic;

public interface IHomeService
{
    void CreateHome(Home home);
    List<Home> GetAllHomes();
    List<Member> GetMembersFromHome(int homeId);
    List<Device> GetDevicesFromHome(int homeId);
    void AddMemberToHome(int homeId,Member member);
}