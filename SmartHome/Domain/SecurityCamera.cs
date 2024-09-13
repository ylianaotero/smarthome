using System.ComponentModel.DataAnnotations;
using IDomain;

namespace Domain;

public enum LocationType
{
    Indoor,
    Outdoor
}
 
public enum SecurityCameraFunctionality
{
    MotionDetection,
    HumanDetection,
}

public class SecurityCamera : IDevice
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public long Model { get; set; }
    public string Description { get; set; }
    public List<string> PhotoURLs { get; set; }
    public ICompany Company { get; set; }
    public bool IsConnected { get; set; }
    public LocationType LocationType { get; set; }
    public List<SecurityCameraFunctionality> Functionalities { get; set; }
    
    public override bool Equals(object? obj)
    {
        return obj is SecurityCamera camera &&
               Name == camera.Name &&
               Model == camera.Model &&
               Description == camera.Description &&
               PhotoURLs.SequenceEqual(camera.PhotoURLs) &&
               Company.Equals(camera.Company) &&
               IsConnected == camera.IsConnected &&
               Functionalities.SequenceEqual(camera.Functionalities) &&
               Id == camera.Id;
    }
}