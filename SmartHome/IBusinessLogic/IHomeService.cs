using Domain;

namespace BusinessLogic.IServices;

public interface IHomeService
{
    void CreateHome(Home home);
    List<Home> GetAllHomes();
    List<Member> GetMembersByHomeId(int homeId);
    List<Device> GetDevicesByHomeId(int homeId);
    void AddMemberToHome(int homeId,Member member);
}