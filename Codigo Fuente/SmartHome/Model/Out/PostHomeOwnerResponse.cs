using Domain.Abstract;
using Domain.Concrete;

namespace Model.Out;

public class PostHomeOwnerResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Surname { get; set; }
    public string Photo { get; set; }
    public List<Role> Roles { get; set; }
    
    public long Id{ get; set; }

    public PostHomeOwnerResponse(User user)
    {
        List<Role> listOfRoles = user.Roles;
        Name = user.Name;
        Email = user.Email;
        Surname = user.Surname;
        Roles = listOfRoles;
        Photo = user.Photo;
        Id = user.Id; 
    }
}