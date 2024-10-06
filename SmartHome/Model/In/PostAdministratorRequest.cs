using Domain.Abstract;
using Domain.Concrete;

namespace Model.In;

public class PostAdministratorRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Surname { get; set; }
    
    public string Photo { get; set; }
    
    public User ToEntity()
    {
        Administrator admin = new Administrator();
        
        return new User
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Roles = new List<Role>{admin},
            Photo = Photo
        };
    }
}