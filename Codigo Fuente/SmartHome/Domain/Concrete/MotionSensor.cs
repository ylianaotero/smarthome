using Domain.Abstract;
using Domain.Enum;

namespace Domain.Concrete;

public class MotionSensor : Device
{
    public sealed override string Kind { get; set; }
    public List<MotionSensorFunctionality>? Functionalities { get; set; }
    
    public MotionSensor()
    {
        Kind = GetType().Name;
    }
}