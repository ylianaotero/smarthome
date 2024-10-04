using Domain;

namespace Model.In;

public class ChangePermissionsRequest
{
    public string MemberEmail { get; set; }
    
    public bool ReceivesNotifications { get; set; }
    
    public MemberDTO ToEntity()
    {
        return new MemberDTO
        {
            MemberEmail = MemberEmail,
            ReceivesNotifications = ReceivesNotifications
        };
    }
}