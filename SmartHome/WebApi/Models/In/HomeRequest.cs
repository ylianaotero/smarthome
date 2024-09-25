using Domain;

namespace WebApi.Models.In;

public class HomeRequest
{
    public string? Street { get; set; }
    public string? DoorNumber { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    public List<Device> Devices { get; set; }

    

    public Func<Home, bool> ToFilter()
    {
        return home => (string.IsNullOrEmpty(Street) || home.Street == Street) &&
                       (string.IsNullOrEmpty(DoorNumber) || home.DoorNumber == int.Parse(DoorNumber));
    }
}