using System.ComponentModel.DataAnnotations;
using CustomExceptions;

namespace Domain;

public class Home
{
    private const string MessageMemberAlreadyExists = "Member already exists"; 
    private const string MessageMemberNotFound = "Member not found"; 
    private const string MessageDeviceAlreadyExists = "Device already exists"; 
    private const string MessageDeviceNotFound = "Device not found";
    
    private List<User> _members;
    
    [Key]
    public int Id { get; set; }
    public long OwnerId { get; set; }
    public string Street { get; set; }
    public int DoorNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public List<User> Members
    {
        get => _members;
        set => _members = value;
    }

    public List<User> GetMembers()
    {
        return Members;
    }

    public List<DeviceUnit> Devices { get; set; }

    public Home()
    {
        OwnerId = OwnerId;
        Street = Street;
        DoorNumber = DoorNumber;
        Latitude = Latitude;
        Longitude = Longitude;
        Members = new List<User>();
        Devices = new List<DeviceUnit>();
    }

    public void AddMember(User member)
    {
        if (MemberExist(member.Email))
        {
            throw new CannotAddItem(MessageMemberAlreadyExists); 
        }
        
        Members.Add(member);
        AddMemberRoleToUser(member);
    }
    
    private void AddMemberRoleToUser(User member)
    {
        member.Roles.Add(new HomeMember()
        {
            Home = this,
        });
    }

    private bool MemberExist(string email)
    {
        User member = Members.FirstOrDefault(m => m.Email == email);
        if (member == null)
        {
            return false; 
        }
        return true; 
    }
    
    public User FindMember(string email)
    {
        if (!MemberExist(email))
        {
            throw new CannotFindItemInList(MessageMemberNotFound ); 
        }
        return Members.FirstOrDefault(m => m.Email == email); 
    }
    
    private HomeMember MemberOfActualHome(User actualUser)
    {
        List<Role> roles = actualUser.GetRoles().FindAll(role => role.GetType().Name == "HomeMember");
        foreach (Role role in roles)
        {
            if (((HomeMember) role).Home.Id == Id)
            {
                return (HomeMember) role;
            }
        }
        return null;
    }
    
    public bool MemberCanReceiveNotifications(string email)
    {
        User user = FindMember(email);
        HomeMember member = MemberOfActualHome(user);
        if(member == null)
        {
            return false;
        }
        return member.ReceivesNotifications;

    }

    public void DeleteMember(string email)
    {
        try
        {
            RemoveMemberRoleFromUser(email);
            Members.Remove(FindMember(email));
        }
        catch
        {
            throw new CannotFindItemInList(MessageMemberNotFound); 
        }
    }
    
    private void RemoveMemberRoleFromUser(string email)
    {
        User user = FindMember(email);
        HomeMember member = MemberOfActualHome(user);
        if(member == null)
        {
            throw new CannotFindItemInList(MessageMemberNotFound);
        }
        user.DeleteRole(member);
    }

    public void AddDevice(DeviceUnit device)
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
        DeviceUnit device = Devices.FirstOrDefault(d => d.Id == id);
        if (device == null)
        {
            return false; 
        }
        return true; 
    }

    public DeviceUnit FindDevice(long id)
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
    
    public bool Equals(Home home)
    {
        return Id == home.Id;
    }
}