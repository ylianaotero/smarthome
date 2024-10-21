using Domain.Abstract;
using Domain.Enum;

namespace Domain.Concrete;

public class SmartLamp : Device
{
    public List<SmartLampFunctionality>? Functionalities { get; set; }
    public sealed override string Kind { get; set; }
    
    public SmartLamp()
    {
        Kind = GetType().Name;
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