using Domain.Concrete;

namespace Model.In;

public class PutHomeOwnerRequest
{
    public string? Name { get; set; }      
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Surname { get; set; }    
    public string? Photo { get; set; }      

    public void UpdateEntity(User user)
    {
        if (!string.IsNullOrEmpty(Name)) user.Name = Name;
        if (!string.IsNullOrEmpty(Email)) user.Email = Email;
        if (!string.IsNullOrEmpty(Password)) user.Password = Password;
        if (!string.IsNullOrEmpty(Surname)) user.Surname = Surname;
        if (!string.IsNullOrEmpty(Photo)) user.Photo = Photo;
    }

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