using Domain.Concrete;
using Domain.Enum;

namespace Model.In;

public class PostSmartLampRequest
{
    public string Name { get; set; }
    public string Model { get; set; }
    public List<string> PhotoUrls { get; set; }
    public string Description { get; set; }
    public List<string>? Functionalities { get; set; }
    public long Company { get; set; }
    
    public SmartLamp ToEntity() 
    {
        return new SmartLamp() 
        {
            Name = this.Name,
            Model = this.Model,
            PhotoURLs = this.PhotoUrls,
            Description = this.Description,
            Functionalities = this.GetFunctionalities(),
        };
    }
    
    private List<SmartLampFunctionality> GetFunctionalities()
    {
        List<SmartLampFunctionality> functionalities = new List<SmartLampFunctionality>();
        
        if (this.Functionalities == null)
        {
            return functionalities;
        }
        
        foreach (var functionality in this.Functionalities)
        {
            foreach (var func in Enum.GetValues(typeof(SmartLampFunctionality)))
            {
                if (func.ToString().ToLower() == functionality.ToLower().Replace(" ", ""))
                {
                    functionalities.Add((SmartLampFunctionality)func);
                    break;
                }
            }
        }
        
        return functionalities;
    }
}