using Domain;

namespace Model.In;

public class CreateHomeRequest
{
    public long OwnerId { get; set; }
    public string Street { get; set; }
    public int DoorNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaximumMembers { get; set; }
    
    public Home ToEntity()
    {
        return new Home()
        {
            OwnerId = this.OwnerId,
            Street = this.Street,
            DoorNumber = this.DoorNumber,
            Latitude = this.Latitude,
            Longitude = this.Longitude
        };
    }
}