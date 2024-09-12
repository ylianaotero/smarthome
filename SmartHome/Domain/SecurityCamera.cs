using IDomain;

namespace Domain;

public class SecurityCamera : IDevice
{
    public string Name { get; set; }
    public long Model { get; set; }
    public List<string> PhotoURLs { get; set; }
    public bool IsConnected { get; set; }
}