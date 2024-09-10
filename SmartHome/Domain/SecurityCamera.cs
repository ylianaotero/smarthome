using IDomain;

namespace Domain;

public class SecurityCamera : IDevice
{
    public bool IsConnected { get; set; }
    public string Name { get; set; }
}