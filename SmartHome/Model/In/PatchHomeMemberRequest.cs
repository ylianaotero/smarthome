using Domain.DTO;

namespace Model.In;

public class PatchHomeMemberRequest
{
    public string MemberEmail { get; set; }
    public bool ReceivesNotifications { get; set; }
    
    public MemberDTO ToEntity()
    {
        return new MemberDTO
        {
            UserEmail = MemberEmail,
            ReceivesNotifications = ReceivesNotifications
        };
    }
}