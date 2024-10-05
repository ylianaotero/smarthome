namespace Domain;

public class Notification
{
    public int Id { get; set; }
    public Home Home { get; set; }
    public Member Member { get; set; }
    public DeviceUnit DeviceUnit { get; set; }
    public string Event { get; set; }
    public DateTime CreatedAt { get; private set; }
    public bool Read { get; private set; }
    public DateTime ReadAt { get; private set; }
    
    public Notification(string newEvent)
    {
        Event = newEvent;
        CreatedAt = DateTime.Now;  
        Read = false;  
        ReadAt = DateTime.MinValue;  
    }
    
    public Notification()
    {
        Event = "";
        CreatedAt = DateTime.Now;  
        Read = false;  
        ReadAt = DateTime.MinValue;  
    }
    
    public void MarkAsRead()
    {
        Read = true;
        ReadAt = DateTime.Now; 
    }
}