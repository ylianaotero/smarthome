using System.ComponentModel.DataAnnotations;
using Domain.Enum;

namespace Domain.Concrete;

public class SmartLamp
{
    [Key]
    public long Id { get; set; }
    public List<SmartLampFunctionality>? Functionalities { get; set; }
    
    public SmartLamp()
    {
    }
}