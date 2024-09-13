using IDomain;

namespace Domain;

public enum WindowSensorFunctionality
{
    OpenClosed
}

public class WindowSensor : IDevice
{
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
               EqualityComparer<List<string>>.Default.Equals(PhotoURLs, sensor.PhotoURLs) &&
               EqualityComparer<ICompany>.Default.Equals(Company, sensor.Company) &&
               IsConnected == sensor.IsConnected &&
               EqualityComparer<List<WindowSensorFunctionality>>.Default.Equals(Functionalities, sensor.Functionalities);
    }
}