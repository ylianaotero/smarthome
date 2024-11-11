using System.ComponentModel.DataAnnotations;
using CustomExceptions;
using Domain.Concrete;

namespace Domain.Abstract;

public abstract class Device
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public List<string> PhotoURLs { get; set; }
    public Company? Company { get; set; }
    public abstract string Kind { get; set; }

    public abstract void ValidateStatus(string status);
    
    public string RunFunctionality(string functionality, string currentStatus)
    {
        if (Kind == "WindowSensor")
        {
            if (functionality != "OpenClosed")
            {
                throw new InputNotValid("Functionality not supported for this device");
            }

            return SwitchStatus(currentStatus);
        }

        if (Kind == "SmartLamp")
        {
            if (functionality != "OnOff")
            {
                throw new InputNotValid("Functionality not supported for this device");
            }
            
            return SwitchStatus(currentStatus);
        }
        
        if (Kind == "SecurityCamera")
        {
            if (functionality != "MotionDetection" && functionality != "HumanDetection")
            {
                throw new InputNotValid("Functionality not supported for this device");
            }
            
            return SwitchStatus(currentStatus);
        }

        if (Kind == "MotionSensor")
        {
            if (functionality != "MotionDetection")
            {
                throw new InputNotValid("Functionality not supported for this device");
            }
            
            return SwitchStatus(currentStatus);
        }

        return currentStatus;
    }
    
    private string SwitchStatus(string currentStatus)
    {
        ValidateStatus(currentStatus);
        
        if (Kind == "WindowSensor")
        {
            if (currentStatus == "Open")
            {
                return "Closed";
            }
            return "Open";
        }
        
        if (Kind == "SmartLamp")
        {
            if (currentStatus == "On")
            {
                return "Off";
            }
            return "On";
        }
        
        if (Kind == "SecurityCamera")
        {
            return "";
        }
        
        return currentStatus;
    }
}