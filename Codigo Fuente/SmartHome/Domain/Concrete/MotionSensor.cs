using System.ComponentModel.DataAnnotations;

namespace Domain.Concrete;

public class MotionSensor
{
    [Key]
    public long Id { get; set; }
    
    public MotionSensor()
    {
        
    }
}