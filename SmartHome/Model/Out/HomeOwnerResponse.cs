using Domain;

namespace Model.Out;

public class HomeOwnerResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    
    public string Surname { get; set; }
    
    public string Photo { get; set; }
    
    public List<Role> Roles { get; set; }

    public HomeOwnerResponse(User user)
    {
        List<Role> listOfRoles = user.Roles;
        Name = user.Name;
        Email = user.Email;
        Surname = user.Surname;
        Roles = listOfRoles;
        Photo = user.Photo; 
    }
}