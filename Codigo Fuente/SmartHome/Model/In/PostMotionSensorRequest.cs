using Domain.Concrete;
using Domain.Enum;

namespace Model.In;

public class PostMotionSensorRequest
{
    public string Name { get; set; }
    public long Model { get; set; }
    public List<string> PhotoUrls { get; set; }
    public string Description { get; set; }
    public List<string>? Functionalities { get; set; }
    public long Company { get; set; }
    
    public MotionSensor ToEntity() 
    {
        return new MotionSensor() 
        {
            Name = this.Name,
            Model = this.Model,
            PhotoURLs = this.PhotoUrls,
            Description = this.Description,
            Functionalities = this.GetFunctionalities(),
        };
    }
    
    private List<MotionSensorFunctionality> GetFunctionalities()
    {
        List<MotionSensorFunctionality> functionalities = new List<MotionSensorFunctionality>();
        
        if (this.Functionalities == null)
        {
            return functionalities;
        }
        
        foreach (var functionality in this.Functionalities)
        {
            foreach (var func in Enum.GetValues(typeof(MotionSensorFunctionality)))
            {
                if (func.ToString().ToLower() == functionality.ToLower().Replace(" ", ""))
                {
                    functionalities.Add((MotionSensorFunctionality)func);
                    break;
                }
            }
        }
        
        return functionalities;
    }
}