namespace Domain;

public class Member
{
    public User User { get; set; }
    public List<Notification> Notifications { get; set; }
    
    public bool HasPermissionToListDevices { get; set; }
    
    public bool HasPermissionToAddADevice { get; set; }
    
    public bool ReceivesNotifications { get; set; }
    
    public Member(User user)
    {
        User = user; 
        Notifications = new List<Notification>();
    }
    
    public void AddNotification(Notification notification)
    {
        Notifications.Add(notification);
    }
    
    public void RemoveNotification(Notification notification)
    {
        Notifications.Remove(notification);
    }
    
    public Notification GetNotificationById(int id)
    {
        return Notifications.FirstOrDefault(n => n.Id == id);
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Member member &&
               Notifications == member.Notifications &&
               ReceivesNotifications == member.ReceivesNotifications &&
               HasPermissionToListDevices == member.HasPermissionToListDevices &&
               HasPermissionToAddADevice == member.HasPermissionToAddADevice &&
               User.Email == member.User.Email; 
    }
}