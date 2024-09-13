using Domain.Exceptions.RoleExceptions;

namespace Domain;

public class User
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<Role> Roles { get; set; }

    public User()
    {
        Roles = new List<Role>();
    }

    public void AddRole(Role role)
    {
        Roles.Add(role);
    }

    public void DeleteRole(Role role)
    {
        if (Roles.Contains(role))
        {
            Roles.Remove(role);
        }
        else
        {
            throw new RoleNotFoundException("The role does not exist");

        }
    }
}