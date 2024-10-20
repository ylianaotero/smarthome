using System.ComponentModel.DataAnnotations;

namespace Domain.Concrete;

public enum MotionSensorFunctionality
{
    MotionDetection
}

public class MotionSensor
{
    [Key]
    public long Id { get; set; }
    
    public List<MotionSensorFunctionality>? Functionalities { get; set; }
    
    public MotionSensor()
    {
        
    }
}