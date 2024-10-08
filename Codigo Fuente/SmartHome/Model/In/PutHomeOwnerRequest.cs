using Domain.Concrete;

namespace Model.In;

public class PutHomeOwnerRequest
{
    public string Name { get; set; }      
    public string Email { get; set; }
    public string Password { get; set; }
    public string Surname { get; set; }    
    public string Photo { get; set; }      

    public User ToEntity()
    {
        return new User
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Photo = Photo
        };
    }
}