using Domain.Concrete;

namespace Model.Out;

public class GetHomeResponse
{
    public string Street { get; set; }
    public int DoorNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public GetHomeResponse(Home home)
    {
        Street = home.Street;
        DoorNumber = home.DoorNumber;
        Latitude = home.Latitude;
        Longitude = home.Longitude;
    }
    public override bool Equals(object? obj)
    {
        return obj is GetHomeResponse response &&
               Street == response.Street &&
               DoorNumber == response.DoorNumber &&
               Latitude == response.Latitude &&
               Longitude == response.Longitude;
    }
}