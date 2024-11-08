using Domain.Concrete;
using Domain.DTO;

namespace IBusinessLogic;

public interface IMemberService
{
    void AddMemberToHome(long homeId, MemberDTO memberDto);
    void UpdateMemberNotificationPermission(long homeId, MemberDTO memberDto);
}