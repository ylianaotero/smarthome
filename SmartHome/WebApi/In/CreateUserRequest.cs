using Domain;

namespace WebApi.In;

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public string Surname { get; set; }

    public User ToEntity()
    {
        return new User
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
        };
    }
}