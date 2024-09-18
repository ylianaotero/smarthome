using System.ComponentModel.DataAnnotations;
using IDomain;

namespace Domain;

public enum WindowSensorFunctionality
{
    OpenClosed
}

public class WindowSensor : Device
{
    public List<WindowSensorFunctionality>? Functionalities { get; set; }
    public sealed override string Kind { get; set; } = "WindowSensor";

    public override bool Equals(object? obj)
    {
        return obj is WindowSensor sensor &&
               Name == sensor.Name &&
               Model == sensor.Model &&
               Description == sensor.Description &&
               PhotoURLs.SequenceEqual(sensor.PhotoURLs) &&
               Company.Equals(sensor.Company) &&
               IsConnected == sensor.IsConnected &&
               Functionalities.SequenceEqual(sensor.Functionalities) &&
               Id == sensor.Id;
    }
}