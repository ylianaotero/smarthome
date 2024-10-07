using Domain.Abstract;
using Domain.Concrete;

namespace Model.Out;

public class PostCompanyOwnerResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Surname { get; set; }
    public List<Role> Roles { get; set; }

    public PostCompanyOwnerResponse(User user)
    {
        List<Role> listOfRoles = user.Roles;
        Name = user.Name;
        Email = user.Email;
        Surname = user.Surname;
        Roles = listOfRoles;
    }
}