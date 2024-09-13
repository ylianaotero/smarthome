using Domain.Exceptions.RoleException;
using IDomain;

namespace Domain;

public class User
{
    private string _name; 
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<IRole> Roles { get; set; }

    private IUserValidator _validator; 
    
    public string Name { 
        get => _name;
        set
        {
            _validator.ValidateName(value);
            _name = value; 
        } 
    }

    public User()
    {
        Roles = new List<IRole>();
        _validator = new UserValidator(); 
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