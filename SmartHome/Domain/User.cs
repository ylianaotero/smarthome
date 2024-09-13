using Domain.Exceptions.GeneralExceptions;
using Domain.Exceptions.RoleException;
using IDomain;

namespace Domain;

public class User
{
    private string _name;
    private string _surname; 
    public string Email { get; set; }
    public string Password { get; set; }
    public List<IRole> Roles { get; set; }

    private IUserValidator _validator; 
    
    public string Name { 
        get => _name;
        set
        {
            bool valid = _validator.ValidateName(value);
            if (valid)
            {
                _name = value; 
            }
            else
            {
                throw new InputNotValid("Input no valido");
            }
            
        } 
    }
    
    public string Surname { 
        get => _surname;
        set
        {
            bool valid = _validator.ValidateSurname(value);
            if (valid)
            {
                _surname = value; 
            }
            else
            {
                throw new InputNotValid("Input no valido");
            }
            
        } 
    }

    
    public User() : this(new UserValidator())
    {
    }

    public User(IUserValidator userValidator)
    {
        Roles = new List<IRole>();
        _validator = userValidator; 
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