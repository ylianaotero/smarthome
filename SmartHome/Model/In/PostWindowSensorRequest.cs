using Domain.Concrete;

namespace Model.In;

public class PostWindowSensorRequest
{
    public string Name { get; set; }
    public long Model { get; set; }
    public List<string> PhotoUrls { get; set; }
    public string Description { get; set; }
    public List<string>? Functionalities { get; set; }
    public long Company { get; set; }
    
    public WindowSensor ToEntity() 
    {
        return new WindowSensor() 
        {
            Name = this.Name,
            Model = this.Model,
            PhotoURLs = this.PhotoUrls,
            Description = this.Description,
            Functionalities = this.GetFunctionalities(),
        };
    }
    
    private List<WindowSensorFunctionality> GetFunctionalities()
    {
        List<WindowSensorFunctionality> functionalities = new List<WindowSensorFunctionality>();
        
        if (this.Functionalities == null)
        {
            return functionalities;
        }
        
        foreach (var functionality in this.Functionalities)
        {
            foreach (var func in Enum.GetValues(typeof(WindowSensorFunctionality)))
            {
                if (func.ToString().ToLower() == functionality.ToLower().Replace(" ", ""))
                {
                    functionalities.Add((WindowSensorFunctionality)func);
                    break;
                }
            }
        }
        
        return functionalities;
    }
}