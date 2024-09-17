using Domain.Exceptions.GeneralExceptions;
using IDomain;

namespace Domain;

public class Home
{
    private List<Member> _members;
    
    public int Id { get; set; }
    public string Street { get; set; }
    public int DoorNumber { get; set; }
    public int Latitude { get; set; }
    public int Longitude { get; set; }
    
    public List<Member> Members
    {
        get => _members;
        private init => _members = value;
    }
    
    public List<Device> Devices { get; set; }
    
    public Home(string street, int doorNumber,int latitude,int longitude)
    {
        Street = street;
        DoorNumber = doorNumber;
        Latitude = latitude;
        Longitude = longitude;
        Members = new();
        Devices = new List<Device>(); 
    }

    public void AddMember(Member member)
    {
        if (MemberExist(member.Email))
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
        Member member = Members.FirstOrDefault(m => m.Email == email);
        if (member == null)
        {
            return false; 
        }
        return true; 
    }
    
    public Member FindMember(string email)
    {
        if (!MemberExist(email))
        {
            throw new CannotFindItemInList("No se encontro el miembro"); 
        }
        return Members.FirstOrDefault(m => m.Email == email); 
    }

    public bool MemberCanReceiveNotifications(string email)
    {
        return FindMember(email).Permission; 
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

    public Device FindDevice(long id)
    {
        if (DeviceExist(id))
        {
            return Devices.FirstOrDefault(d => d.Id == id); 
        }
        throw new CannotFindItemInList("No se encontro el dispositivo"); 
        
    }

    public void DeleteDevice(long id)
    {
        Devices.Remove(FindDevice(id));
    }
}