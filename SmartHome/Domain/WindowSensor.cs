namespace Domain;

public enum WindowSensorFunctionality
{
    OpenClosed
}

public class WindowSensor
{
    public string Name { get; set; }
    public long Model { get; set; }
    public string Description { get; set; }
    public List<string> PhotoURLs { get; set; }
    public Company Company { get; set; }
    public bool IsConnected { get; set; }
    public List<WindowSensorFunctionality> Functionalities { get; set; }
}