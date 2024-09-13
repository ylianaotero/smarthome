using IDomain;

namespace Domain;

public enum LocationType
{
    Indoor,
    Outdoor
}

public class SecurityCamera : IDevice
{
    public string Name { get; set; }
    public long Model { get; set; }
    public List<string> PhotoURLs { get; set; }
    public bool IsConnected { get; set; }
    public Company Company { get; set; }
    public string Description { get; set; }
    public LocationType LocationType { get; set; }
    public bool HasMovementDetection { get; set; }
    
}