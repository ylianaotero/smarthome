using Domain;

namespace WebApi.Out;

public class UserResponse
{
    public string Name { get; set; }
    public string Surname { get; set; }
    
    public string FullName { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public List<Role> Roles { get; set; }

    public UserResponse(User user)
    {
        Name = user.Name;
        Surname = user.Surname;
        FullName = user.Name + " " + user.Surname;
        CreatedAt = user.CreatedAt; 
        Roles =  LoadRolesList(user.Roles);
    }

    private List<Role> LoadRolesList(List<Role> listOfRoles)
    {
        return listOfRoles?.ToList() ?? new List<Role>();
    }
}