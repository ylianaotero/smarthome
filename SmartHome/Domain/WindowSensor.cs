using System.ComponentModel.DataAnnotations;
using IDomain;

namespace Domain;

public enum WindowSensorFunctionality
{
    OpenClosed
}

public class WindowSensor : IDevice
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public long Model { get; set; }
    public string Description { get; set; }
    public List<string> PhotoURLs { get; set; }
    public ICompany Company { get; set; }
    public bool IsConnected { get; set; }
    public List<WindowSensorFunctionality> Functionalities { get; set; }

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