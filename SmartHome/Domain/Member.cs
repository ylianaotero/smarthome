namespace Domain;

public class Member : User
{
    public bool Permission { get; set; }
    
    public bool ReceivesNotifications { get; set; }
    
    private List<Notification> Notifications { get; set; }
    
    public Member()
    {
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

    public List<Notification> GetAllNotifications()
    {
        return Notifications;
    }
}