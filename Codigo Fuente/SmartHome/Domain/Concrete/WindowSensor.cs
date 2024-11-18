using CustomExceptions;
using Domain.Abstract;
using Domain.Enum;

namespace Domain.Concrete;

public class WindowSensor : Device
{
    private const string WindowSensorStatusMessage = "WindowSensor status can only be on or off.";
    private const string WindowSensorFunctionalityMessage = "Functionality not supported for this device.";
    
    public List<WindowSensorFunctionality>? Functionalities { get; set; }
    public sealed override string Kind { get; set; }

    public WindowSensor()
    {
        Kind = GetType().Name;
    }
    
    public override void ValidateStatus(string status)
    {
        if (status != "Open" && status != "Closed" && !String.IsNullOrEmpty(status))
        {
            throw new InputNotValid(WindowSensorStatusMessage);
        }
    }
    
    public override string RunFunctionality(string functionality, string currentStatus)
    {
        if (!FunctionalityIsValid(functionality))
        {
            throw new InputNotValid(WindowSensorFunctionalityMessage);
        }
            
        return SwitchStatus(currentStatus);
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

        foreach (WindowSensorFunctionality item in Functionalities)
        {
            if (item.ToString() == functionality)
            {
                isSupported = true;
            }
        }

        return isSupported;
    }
    
    private string SwitchStatus(string currentStatus)
    {
        if (currentStatus == "Open")
        {
            return "Closed";
        }
        
        return "Open";
    }
    
    public override bool Equals(object? obj)
    {
        return obj is WindowSensor sensor &&
               Name == sensor.Name &&
               Model == sensor.Model &&
               Description == sensor.Description &&
               PhotoURLs.SequenceEqual(sensor.PhotoURLs) &&
               Company.Equals(sensor.Company) &&
               Functionalities.SequenceEqual(sensor.Functionalities) &&
               Id == sensor.Id;
    }
}