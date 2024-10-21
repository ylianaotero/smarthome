using System.ComponentModel.DataAnnotations;
using Domain.Abstract;
using Domain.Enum;

namespace Domain.Concrete;

public class MotionSensor : Device
{
    [Key]
    public long Id { get; set; }
    public sealed override string Kind { get; set; }
    
    public List<MotionSensorFunctionality>? Functionalities { get; set; }
    
    public MotionSensor()
    {
        Kind = GetType().Name;
    }
}