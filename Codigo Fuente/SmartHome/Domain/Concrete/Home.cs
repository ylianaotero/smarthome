using System.ComponentModel.DataAnnotations;
using CustomExceptions;

namespace Domain.Concrete;

public class Home
{
    private const string MessageMemberAlreadyExists = "Member already exists"; 
    private const string MessageMemberNotFound = "Member not found"; 
    private const string MessageDeviceAlreadyExists = "Device already exists"; 
    private const string MessageDeviceNotFound = "Device not found";
    private const string MessageRoomByNameAlreadyExists = "A room with the same name has already been added to this home";
    private const string InputNotValidMessage = "Home alias cannot be empty";

    private string _alias;
    
    [Key]
    public long Id { get; set; }
    public User Owner { get; set; }
    public string Alias
    {
        get => _alias;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InputNotValid(InputNotValidMessage); 
            }
            _alias = value;
        }
    }

    public string Street { get; set; }
    public int DoorNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaximumMembers { get; set; }
    public List<Room> Rooms { get; set; }
    public List<DeviceUnitService> Devices { get; set; }
    public List<Member> Members { get; set; }

    public Home()
    {
        Members = new List<Member>();
        Devices = new List<DeviceUnitService>();
        Rooms = new List<Room>();
    }
    
    public void AddMember(Member member)
    {
        if (MemberExists(member.User.Email))
        {
            throw new CannotAddItem(MessageMemberAlreadyExists); 
        }
        
        Members.Add(member);
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
    
    public Member FindMember(string email)
    {
        if (!MemberExists(email))
        {
            throw new CannotFindItemInList(MessageMemberNotFound ); 
        }
        
        return Members.Find(m => m.User.Email == email); 
    }

    public void AddDevice(DeviceUnitService device)
    {
        if (!DeviceExists(device.HardwareId))
        {
            Devices.Add(device);
        }
        else
        {
            throw new CannotAddItem(MessageDeviceAlreadyExists); 
        }
    }
    
    public DeviceUnitService FindDevice(Guid id)
    {
        if (DeviceExists(id))
        {
            return Devices.Find(d => d.HardwareId == id); 
        }
        
        throw new CannotFindItemInList(MessageDeviceNotFound ); 
    }

    public void DeleteDevice(Guid id)
    {
        Devices.Remove(FindDevice(id));
    }
    
    
    public void AddRoom(Room room)
    {
        if (RoomExists(room))
        {
            throw new ElementAlreadyExist(MessageRoomByNameAlreadyExists);
        }
        
        Rooms.Add(room);
    }
    private bool MemberExists(string email)
    {
        return Members.Exists(m => m.User.Email == email);
    }
    
    private bool RoomExists(Room room)
    {
        return Rooms.Exists(r => r.Name == room.Name) || Rooms.Exists(r => r.Id == room.Id);
    }
    
    private bool DeviceExists(Guid id)
    {
        return Devices.Exists(d => d.HardwareId == id);
    }
}