namespace Domain.Concrete;

public class Notification
{
    public int Id { get; set; }
    public Home Home { get; set; }
    public Member Member { get; set; }
    public DeviceUnit DeviceUnit { get; set; }
    public string Event { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Read { get; set; }
    public DateTime ReadAt { get; set; }
    
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