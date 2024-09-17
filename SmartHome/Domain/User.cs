
using System.ComponentModel.DataAnnotations;
using Domain.Exceptions.RoleExceptions;
using Domain.Exceptions.GeneralExceptions;
using IDomain;

namespace Domain;

public class User
{
    [Key]
    public long Id { get; set; }
    private string _email { get; set; }
    private string _name { get; set; }
    private string _surname { get; set; }
    private string _password { get; set; }
    public List<Role> Roles { get; set; }

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
    
    public string Password { 
        get => _password;
        set
        {
            bool valid = _validator.ValidatePassword(value);
            if (valid)
            {
                _password = value; 
            }
            else
            {
                throw new InputNotValid("Input no valido");
            }
            
        } 
    }
    
    public string Email { 
        get => _email;
        set
        {
            bool valid = _validator.ValidateEmail(value);
            if (valid)
            {
                _email = value; 
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
        Roles = new List<Role>();
        _validator = userValidator; 
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