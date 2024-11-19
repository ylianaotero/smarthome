using CustomExceptions;
using Domain.Abstract;
using Domain.Enum;

namespace Domain.Concrete;

public class MotionSensor : Device
{
    private const string MotionSensorStatusMessage = "MotionSensor status cannot be set.";
    private const string MotionSensorFunctionalityMessage = "Functionality not supported for this device.";
    
    public sealed override string Kind { get; set; }
    public List<MotionSensorFunctionality>? Functionalities { get; set; }
    
    public MotionSensor()
    {
        Kind = GetType().Name;
    }
    
    public override void ValidateStatus(string status)
    {
        if (!String.IsNullOrEmpty(status))
        {
            throw new InputNotValid(MotionSensorStatusMessage);
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
            throw new InputNotValid(MotionSensorFunctionalityMessage);
        }

        return DefaultStatus();
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

        foreach (MotionSensorFunctionality item in Functionalities)
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
        return obj is MotionSensor sensor &&
               Name == sensor.Name &&
               Model == sensor.Model &&
               Description == sensor.Description &&
               PhotoURLs.SequenceEqual(sensor.PhotoURLs) &&
               Company.Equals(sensor.Company) &&
               Functionalities.SequenceEqual(sensor.Functionalities) &&
               Id == sensor.Id;
    }
}