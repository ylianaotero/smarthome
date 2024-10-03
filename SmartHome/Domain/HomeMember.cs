namespace Domain;

public class HomeMember : Role
{
    private List<Notification> Notifications { get; set; }
    public Home Home { get; set; }
    
    public bool HasPermissionToAddADevice { get; set; }
    
    public bool ReceivesNotifications { get; set; }

    public HomeMember()
    {
        Notifications = new List<Notification>();
        HasPermissionToAddADevice = true;
        ReceivesNotifications = true; //Se deja asi?
    }
    
    public void AddNotification(Notification notification)
    {
        Notifications.Add(notification);
    }
    
    public List<Notification> GetNotifications()
    {
        return Notifications;
    }
}