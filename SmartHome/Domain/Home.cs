using Domain.Exceptions.GeneralExceptions;
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

    public void AddMember((User, bool) member)
    {
        Members.Add(member);
    }
    
    public (User,bool) FindMember(string email)
    {
        (User,bool) member = Members.FirstOrDefault(m => m.Item1.Email == email);
        if (member.Item1 == null)
        {
            throw new CannotFindItemInList("No se encontro el miembro"); 
        }
        return member; 
    }

    public bool MemberCanReceiveNotifications(string email)
    {
        return FindMember(email).Item2; 
    }

    public void DeleteMember(string email)
    {
        Members.Remove(FindMember(email));
    }
}