using Domain.Concrete;
using Domain.Enum;

namespace Model.In;

public class PostSecurityCameraRequest
{
    public string Name { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public List<string> PhotoUrls { get; set; }
    public string? LocationType { get; set; }
    public List<string>? Functionalities { get; set; }
    public long Company { get; set; }
    
    public SecurityCamera ToEntity() 
    {
        return new SecurityCamera() 
        {
            Name = this.Name,
            Model = this.Model,
            Description = this.Description,
            PhotoURLs = this.PhotoUrls,
            LocationType = this.GetLocationType(),
            Functionalities = GetFunctionalities(),
        };
    }
    
    private List<SecurityCameraFunctionality> GetFunctionalities()
    {
        List<SecurityCameraFunctionality> functionalities = new List<SecurityCameraFunctionality>();
        
        if (this.Functionalities == null)
        {
            return functionalities;
        }
        
        foreach (var functionality in this.Functionalities)
        {
            foreach (var func in Enum.GetValues(typeof(SecurityCameraFunctionality)))
            {
                if (func.ToString().ToLower() == functionality.ToLower().Replace(" ", ""))
                {
                    functionalities.Add((SecurityCameraFunctionality)func);
                    break;
                }
            }
        }
        
        return functionalities;
    }
    
    private LocationType? GetLocationType()
    {
        foreach (var locationType in Enum.GetValues(typeof(LocationType)))
        {
            if (locationType.ToString().ToLower() == this.LocationType.ToLower().Replace(" ", ""))
            {
                return (LocationType)locationType;
            }
        }

        return null;
    }
}