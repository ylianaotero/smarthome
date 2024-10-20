using Domain.Abstract;

namespace Domain.Concrete;

public enum MovementSensorFunctionality
{
    MotionDetection
}

public class MovementSensor : Device
{
    public List<MovementSensorFunctionality>? Functionalities { get; set; }
    public override string Kind { get; set; }
    
    public MovementSensor()
    {
        Kind = "MovementSensor";
    }
}