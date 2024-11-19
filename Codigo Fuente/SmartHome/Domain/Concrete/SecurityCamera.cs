using CustomExceptions;
using Domain.Abstract;
using Domain.Enum;

namespace Domain.Concrete;

public class SecurityCamera : Device
{
    private const string SecurityCameraStatusMessage = "SecurityCamera status cannot be set.";
    private const string SecurityCameraFunctionalityMessage = "Functionality not supported for this device.";
    
    public LocationType? LocationType { get; set; }
    public List<SecurityCameraFunctionality>? Functionalities { get; set; }
    public sealed override string Kind { get; set; }
    
    public SecurityCamera()
    {
        Kind = GetType().Name;
    }
    
    public override void ValidateStatus(string status)
    {
        if (!String.IsNullOrEmpty(status))
        {
            throw new InputNotValid(SecurityCameraStatusMessage);
        }
    }
    
    public override string DefaultStatus()
    {
        return "";
    }
    
    public override string RunFunctionality(string functionality, string currentStatus)
    {
        if (!FunctionalityIsValid(functionality))
        {
            throw new InputNotValid(SecurityCameraFunctionalityMessage);
        }
            
        return currentStatus;
    }
    
    private bool FunctionalityIsValid(string functionality)
    {
        bool isValid = false;

        if (Functionalities != null)
        {
            isValid = FunctionalityIsSupportedByDevice(functionality);
        }

        return isValid;
    }
    
    private bool FunctionalityIsSupportedByDevice(string functionality)
    {
        bool isSupported = false;

        foreach (SecurityCameraFunctionality item in Functionalities)
        {
            if (item.ToString() == functionality)
            {
                isSupported = true;
            }
        }

        return isSupported;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is SecurityCamera camera && 
               Id == camera.Id;
    }
}