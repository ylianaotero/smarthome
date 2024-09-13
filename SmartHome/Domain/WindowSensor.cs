namespace Domain;

public enum WindowSensorFunctionality
{
    OpenClosed
}

public class WindowSensor
{
    public List<WindowSensorFunctionality> Functionalities { get; set; }
}