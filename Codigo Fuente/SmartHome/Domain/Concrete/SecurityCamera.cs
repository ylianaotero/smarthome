using Domain.Abstract;

namespace Domain.Concrete;

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

public class SecurityCamera : Device
{
    public LocationType? LocationType { get; set; }
    public List<SecurityCameraFunctionality>? Functionalities { get; set; }
    public sealed override string Kind { get; set; }
    
    public SecurityCamera()
    {
        Kind = GetType().Name;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is SecurityCamera camera && 
               Id == camera.Id;
    }
}