using CustomExceptions;
using Domain.Abstract;
using Domain.Enum;
using Microsoft.VisualBasic;

namespace Domain.Concrete;

public class SmartLamp : Device
{
    private const string SmartLampStatusMessage = "SmartLamp status can only be on or off.";
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