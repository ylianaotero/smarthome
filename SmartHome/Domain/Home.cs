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
        if (MemberExist(member.Item1.Email))
        {
            throw new CannotAddItem("El miembro ya existe"); 
        }
        else
        {
            Members.Add(member);
        }
        
    }

    private bool MemberExist(string email)
    {
        (User,bool) member = Members.FirstOrDefault(m => m.Item1.Email == email);
        if (member.Item1 == null)
        {
            return false; 
        }
        return true; 
    }
    
    public (User,bool) FindMember(string email)
    {
        if (!MemberExist(email))
        {
            throw new CannotFindItemInList("No se encontro el miembro"); 
        }
        return Members.FirstOrDefault(m => m.Item1.Email == email); 
    }

    public bool MemberCanReceiveNotifications(string email)
    {
        return FindMember(email).Item2; 
    }

    public void DeleteMember(string email)
    {
        Members.Remove(FindMember(email));
    }

    public void AddDevice(Device device)
    {
        if (!DeviceExist(device.Id))
        {
            Devices.Add(device);
        }
        else
        {
            throw new CannotAddItem("El dispositivo ya existe"); 
        }
        
    }
    
    private bool DeviceExist(long id)
    {
        Device device = Devices.FirstOrDefault(d => d.Id == id);
        if (device == null)
        {
            return false; 
        }
        return true; 
    }
}