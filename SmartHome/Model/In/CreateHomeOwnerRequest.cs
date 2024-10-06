using Domain;
using Domain.Abstract;
using Domain.Concrete;

namespace Model.In;

public class CreateHomeOwnerRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public string Surname { get; set; }
    
    public string Photo { get; set; }

    public User ToEntity()
    {
        HomeOwner homeOwner = new HomeOwner();
        
        return new User
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = Photo,
            Roles = new List<Role>{homeOwner}
        };
    }
}