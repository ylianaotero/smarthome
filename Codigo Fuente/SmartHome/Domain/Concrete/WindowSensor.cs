using CustomExceptions;
using Domain.Abstract;
using Domain.Enum;

namespace Domain.Concrete;

public class WindowSensor : Device
{
    private const string WindowSensorStatusMessage = "WindowSensor status can only be on or off.";
    public List<WindowSensorFunctionality>? Functionalities { get; set; }
    public sealed override string Kind { get; set; }

    public WindowSensor()
    {
        Kind = GetType().Name;
    }
    
    public override void ValidateStatus(string status)
    {
        if (status != "Open" && status != "Closed" && !String.IsNullOrEmpty(status))
        {
            throw new InputNotValid(WindowSensorStatusMessage);
        }
    }
    
    public override bool Equals(object? obj)
    {
        return obj is WindowSensor sensor &&
               Name == sensor.Name &&
               Model == sensor.Model &&
               Description == sensor.Description &&
               PhotoURLs.SequenceEqual(sensor.PhotoURLs) &&
               Company.Equals(sensor.Company) &&
               Functionalities.SequenceEqual(sensor.Functionalities) &&
               Id == sensor.Id;
    }
}