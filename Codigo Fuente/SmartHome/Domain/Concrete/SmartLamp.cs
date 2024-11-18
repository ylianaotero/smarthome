using CustomExceptions;
using Domain.Abstract;
using Domain.Enum;
using Microsoft.VisualBasic;

namespace Domain.Concrete;

public class SmartLamp : Device
{
    private const string SmartLampStatusMessage = "SmartLamp status can only be on or off.";
    private const string SmartLampFunctionalityMessage = "Functionality not supported for this device.";
    
    public List<SmartLampFunctionality>? Functionalities { get; set; }
    public sealed override string Kind { get; set; }
    
    public SmartLamp()
    {
        Kind = GetType().Name;
    }
    
    public override void ValidateStatus(string status)
    {
        if (status != "On" && status != "Off" && !String.IsNullOrEmpty(status))
        {
            throw new InputNotValid(SmartLampStatusMessage);
        }
    }
    
    public override string DefaultStatus()
    {
        return "Off";
    }
    
    public override string RunFunctionality(string functionality, string currentStatus)
    {
        if (!FunctionalityIsValid(functionality))
        {
            throw new InputNotValid(SmartLampFunctionalityMessage);
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

        foreach (SmartLampFunctionality item in Functionalities)
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
        if (currentStatus == "On")
        {
            return "Off";
        }
        
        return "On";
    }

    public override bool Equals(object? obj)
    {
        return obj is SmartLamp lamp &&
               Name == lamp.Name &&
               Model == lamp.Model &&
               Description == lamp.Description &&
               PhotoURLs.SequenceEqual(lamp.PhotoURLs) &&
               Company.Equals(lamp.Company) &&
               Functionalities.SequenceEqual(lamp.Functionalities) &&
               Id == lamp.Id;
    }
}