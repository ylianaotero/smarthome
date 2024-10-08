using Domain.DTO;

namespace Model.In;

public class PostHomeMemberRequest
{
    public string UserEmail { get; set; }
    
    public bool HasPermissionToListDevices { get; set; }
    
    public bool HasPermissionToAddADevice { get; set; }
    
    public bool ReceivesNotifications { get; set; }
    
    public MemberDTO ToEntity()
    {
        return new MemberDTO
        {
            UserEmail = UserEmail, 
            ReceivesNotifications = ReceivesNotifications, 
            HasPermissionToListDevices = HasPermissionToListDevices,
            HasPermissionToAddADevice = HasPermissionToAddADevice
        };
    }
}