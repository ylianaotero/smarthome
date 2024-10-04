using Domain;

namespace Model.In;

public class MemberRequest
{
    public string UserEmail { get; set; }
    
    public bool HasPermissionToListDevices { get; set; }
    
    public bool HasPermissionToAddADevice { get; set; }
    
    public bool ReceivesNotifications { get; set; }
    
    public MemberDTO ToEntity()
    {
        MemberDTO memberDTO = new MemberDTO();
        
        return new MemberDTO
        {
            UserEmail = UserEmail, 
            ReceivesNotifications = ReceivesNotifications, 
            HasPermissionToListDevices = HasPermissionToListDevices,
            HasPermissionToAddADevice = HasPermissionToAddADevice
        };
    }
}