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

    public override bool Equals(object? obj)
    {
        return obj is MotionSensor sensor &&
               Name == sensor.Name &&
               Model == sensor.Model &&
               Description == sensor.Description &&
               PhotoURLs.SequenceEqual(sensor.PhotoURLs) &&
               Company.Equals(sensor.Company) &&
               Functionalities.SequenceEqual(sensor.Functionalities) &&
               Id == sensor.Id;
    }
}