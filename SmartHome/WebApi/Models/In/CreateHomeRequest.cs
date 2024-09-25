using Domain;

namespace WebApi.Models.In;

public class CreateHomeRequest
{
    public long OwnerId { get; set; }
    public string Street { get; set; }
    public int DoorNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<Member>? Members { get; set; }
}