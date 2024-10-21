using System.ComponentModel.DataAnnotations;
using Domain.Enum;

namespace Domain.Concrete;

public class MotionSensor
{
    [Key]
    public long Id { get; set; }
    public string Kind { get; set; }
    
    public List<MotionSensorFunctionality>? Functionalities { get; set; }
    
    public MotionSensor()
    {
        
    }
}