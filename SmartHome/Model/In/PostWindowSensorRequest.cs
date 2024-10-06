using Domain.Concrete;

namespace Model.In;

public class PostWindowSensorRequest
{
    public string Name { get; set; }
    public long Model { get; set; }
    public List<string> PhotoUrls { get; set; }
    public string Description { get; set; }
    public List<WindowSensorFunctionality>? Functionalities { get; set; }
    public long Company { get; set; }
    
    public WindowSensor ToEntity() 
    {
        return new WindowSensor() 
        {
            Name = this.Name,
            Model = this.Model,
            PhotoURLs = this.PhotoUrls,
            Description = this.Description,
            Functionalities = this.Functionalities,
        };
    }
}