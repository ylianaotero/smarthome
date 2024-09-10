namespace Domain;

public class SecurityCamera
{
    public bool IsConnected { get; private set; }
    
    public void Connect()
    {
        IsConnected = true;
    }
}