using CustomExceptions;
using Domain.Abstract;
using Domain.Enum;
using Microsoft.VisualBasic;

namespace Domain.Concrete;

public class MotionSensor : Device
{
    private const string MotionSensorStatusMessage = "MotionSensor status cannot be set.";
    public sealed override string Kind { get; set; }
    public List<MotionSensorFunctionality>? Functionalities { get; set; }
    
    public MotionSensor()
    {
        Kind = GetType().Name;
    }
    
    public override void ValidateStatus(string status)
    {
        if (!String.IsNullOrEmpty(status))
        {
            throw new InputNotValid(MotionSensorStatusMessage);
        }
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