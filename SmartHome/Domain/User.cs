using Domain.DomainExceptions.RoleException;
using IDomain;

namespace Domain;

public class User
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<IRole> Roles { get; set; }

    public User()
    {
        Roles = new List<IRole>();
    }

    public void AddRole(IRole role)
    {
        Roles.Add(role);
    }

    public void DeleteRole(IRole role)
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