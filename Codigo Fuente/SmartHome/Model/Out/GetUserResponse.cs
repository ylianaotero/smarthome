using Domain.Abstract;
using Domain.Concrete;

namespace Model.Out;

public class GetUserResponse
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string FullName { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Role> Roles { get; set; }
    
    public long Id { get; set; }

    public GetUserResponse(User user)
    {
        Name = user.Name;
        Surname = user.Surname;
        FullName = user.Name + " " + user.Surname;
        CreatedAt = user.CreatedAt; 
        Roles =  LoadRolesList(user.Roles);
        Id = user.Id; 
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetUserResponse response &&
               Name == response.Name &&
               Surname == response.Surname &&
               FullName == response.FullName &&
               CreatedAt == response.CreatedAt &&
               Roles.SequenceEqual(response.Roles);
    }

    private List<Role> LoadRolesList(List<Role> listOfRoles)
    {
        return listOfRoles?.ToList() ?? new List<Role>();
    }
}