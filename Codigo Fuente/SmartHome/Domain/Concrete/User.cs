using System.ComponentModel.DataAnnotations;
using CustomExceptions;
using Domain.Abstract;

namespace Domain.Concrete;

public class User
{
    private const string MessageRoleNotFound = "The role does not exist"; 
    private const string MessageInvalidName = "The name is invalid";
    private const string MessageInvalidSurname = "The surname is invalid";
    private const string MessageInvalidPassword = "The password is invalid";
    private const string MessageInvalidEmail = "The email is invalid";
    
    [Key]
    public long Id { get; set; }
    private string _email { get; set; }
    private string _name { get; set; }
    private string _surname { get; set; }
    private string _password { get; set; }
    public List<Role>? Roles { get; set; }
    public DateTime CreatedAt { get; private set; }
    public string? Photo { get; set; }

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
                throw new InputNotValid(MessageInvalidName);
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
                throw new InputNotValid(MessageInvalidSurname);
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
                throw new InputNotValid(MessageInvalidPassword);
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
                throw new InputNotValid(MessageInvalidEmail);
            }
            
        } 
    }
    
    public User() : this(new UserValidator()) {}

    public User(IUserValidator userValidator)
    {
        Roles = new List<Role>();
        _validator = userValidator; 
        CreatedAt = DateTime.Now;
    }

    public void AddRole(Role role)
    {
        Roles.Add(role);
    }

    /*
    public void DeleteRole(Role role)
    {
        if (Roles.Contains(role))
        {
            Roles.Remove(role);
        }
        else
        {
            throw new ElementNotFound(MessageRoleNotFound);
        }
    }*/
    
    public void DeleteRole(Role role)
    {
        Role roleToDelete = Roles.FirstOrDefault(r => r.Id == role.Id);
        if (roleToDelete != null)
        {
            Roles.Remove(roleToDelete);
        }
        else
        {
            throw new ElementNotFound(MessageRoleNotFound);
        }
    }



    public void Update(User user)
    {
        Name = user.Name;
        Surname = user.Surname;
        Password = user.Password;
        Email = user.Email;
        Photo = user.Photo;
    }
}