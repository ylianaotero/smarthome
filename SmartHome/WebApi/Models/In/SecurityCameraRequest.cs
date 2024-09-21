using Domain;

namespace WebApi.In;

public class SecurityCameraRequest
{
    public string Name { get; set; }
    public long Model { get; set; }
    public string Description { get; set; }
    public List<string> PhotoUrls { get; set; }
    public LocationType? LocationType { get; set; }
    public List<SecurityCameraFunctionality> Functionalities { get; set; }
    public Company? Company { get; set; }
}