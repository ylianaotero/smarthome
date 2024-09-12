namespace Domain;

public class Notification
{
    public int Id { get; set; }
    public string Event { get; private set; }
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
    
    public void MarkAsRead()
    {
        Read = true;
        ReadAt = DateTime.Now; 
    }
}