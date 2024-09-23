using Domain;

namespace WebApi.In;

public class WindowSensorRequest
{
    public string Name { get; set; }
    public long Model { get; set; }
    public List<string> PhotoUrls { get; set; }
    public string Description { get; set; }
    public List<WindowSensorFunctionality>? Functionalities { get; set; }
    public Company? Company { get; set; }
}