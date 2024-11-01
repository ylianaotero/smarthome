using Domain.Concrete;

namespace Model.In;

public class PostHomeRequest
{
    public long OwnerId { get; set; }
    public string Alias { get; set; }
    public string Street { get; set; }
    public int DoorNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaximumMembers { get; set; }
    
    public Home ToEntity()
    {
        Home home = new Home()
        {
            Street = this.Street,
            Alias = this.Alias,
            DoorNumber = this.DoorNumber,
            Latitude = this.Latitude,
            Longitude = this.Longitude,
            MaximumMembers = this.MaximumMembers
        };

        return home;
    }
}