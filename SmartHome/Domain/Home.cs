using System.ComponentModel.DataAnnotations;
using CustomExceptions;

namespace Domain;

public class Home
{
    private const string MessageMemberAlreadyExists = "Member already exists"; 
    private const string MessageMemberNotFound = "Member not found"; 
    private const string MessageDeviceAlreadyExists = "Device already exists"; 
    private const string MessageDeviceNotFound = "Device not found";
    
    
    
    [Key]
    public int Id { get; set; }
    public long OwnerId { get; set; }
    public string Street { get; set; }
    public int DoorNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<Member> Members { get; set; }
    public List<Device> Devices { get; set; }
    
    // cantidad de miembros

    public Home()
    {
        Members = new List<Member>();
        Devices = new List<Device>();
    }

    public void AddMember(Member member)
    {
        if (MemberExist(member.User.Email))
        {
            throw new CannotAddItem(MessageMemberAlreadyExists); 
        }
        Members.Add(member);
    }
    

    public bool MemberExist(string email)
    {
        Member member = Members.FirstOrDefault(m => m.User.Email == email);
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
            throw new CannotFindItemInList(MessageMemberNotFound ); 
        }
        return Members.FirstOrDefault(m => m.User.Email == email); 
    }
    
    public bool MemberCanReceiveNotifications(string email)
    {
        Member member = FindMember(email);
        return member.ReceivesNotifications; 

    }

    public void DeleteMember(string email)
    {
        try
        {
            Members.Remove(FindMember(email));
        }
        catch
        {
            throw new CannotFindItemInList(MessageMemberNotFound); 
        }
    }
    public void AddDevice(Device device)
    {
        if (!DeviceExist(device.Id))
        {
            Devices.Add(device);
        }
        else
        {
            throw new CannotAddItem(MessageDeviceAlreadyExists); 
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
        throw new CannotFindItemInList(MessageDeviceNotFound ); 
        
    }

    public void DeleteDevice(long id)
    {
        Devices.Remove(FindDevice(id));
    }
}