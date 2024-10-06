using Domain.Concrete;

namespace Model.Out;

public class MemberResponse
{
    public string FullName { get; set; }
    public string Email { get; set; }
    
    public string Photo { get; set; }
    
    public bool HasPermissionToListDevices { get; set; }
    
    public bool HasPermissionToAddADevice { get; set; }
    
    public bool ReceivesNotifications { get; set; }

    public MemberResponse(Member member)
    {
        FullName = member.User.Name + " " + member.User.Surname;
        Email = member.User.Email;
        Photo = member.User.Photo;
        HasPermissionToListDevices = member.HasPermissionToListDevices;
        HasPermissionToAddADevice = member.HasPermissionToAddADevice;
        ReceivesNotifications = member.ReceivesNotifications;
    }

    public override bool Equals(object? obj)
    {
    
        return obj is MemberResponse response  &&
                      FullName == response.FullName &&
                      Email == response.Email &&
                      Photo == response.Photo &&
                      HasPermissionToListDevices == response.HasPermissionToListDevices &&
                      HasPermissionToAddADevice == response.HasPermissionToAddADevice &&
                      ReceivesNotifications == response.ReceivesNotifications;
    }
}