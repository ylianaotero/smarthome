using System.ComponentModel.DataAnnotations;

namespace Domain.Concrete;

public enum SmartLampFunctionality
{
    OnOff
}

public class SmartLamp
{
    [Key]
    public long Id { get; set; }
    public List<SmartLampFunctionality>? Functionalities { get; set; }
    
    public SmartLamp()
    {
    }
}