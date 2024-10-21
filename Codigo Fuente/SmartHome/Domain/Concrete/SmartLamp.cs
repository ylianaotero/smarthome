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
}