namespace Domain;

public class HomeMember : Role
{
    public List<Notification> Notifications { get; set; }
    public Home Home { get; set; }
    
    public bool HasPermissionToListDevices { get; set; }
    
    public bool HasPermissionToAddADevice { get; set; }
    
    public bool ReceivesNotifications { get; set; }

    public HomeMember()
    {
        Notifications = new List<Notification>();
        HasPermissionToAddADevice = true;
        ReceivesNotifications = false;
        HasPermissionToListDevices = true; 
    }
    
    public void AddNotification(Notification notification)
    {
        Notifications.Add(notification);
    }
}