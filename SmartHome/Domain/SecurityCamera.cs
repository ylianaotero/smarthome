using IDomain;

namespace Domain;

public class SecurityCamera : IDevice
{
    public string Name { get; set; }
    public bool IsConnected { get; set; }
}