using IDomain;

namespace Domain;

public class Home
{
    public int Id { get; set; }
    public string Street { get; set; }
    public int DoorNumber { get; set; }
    public int Latitude { get; set; }
    public int Longitude { get; set; }
    
    public List<(User,bool)> Members { get; set; }
    
    public List<Device> Devices { get; set; }
    
    public Home(string street, int doorNumber,int latitude,int longitude)
    {
        Street = street;
        DoorNumber = doorNumber;
        Latitude = latitude;
        Longitude = longitude;
        Members = new List<(User, bool)>();
        Devices = new List<Device>(); 
    }

}