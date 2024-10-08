using Domain.Concrete;

namespace Model.Out;

public class PostAdministratorResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Surname { get; set; }

    public PostAdministratorResponse(User user)
    {
        Name = user.Name;
        Email = user.Email;
        Surname = user.Surname;
    }

    public override bool Equals(object? obj)
    {
        return obj is PostAdministratorResponse response &&
               Name == response.Name &&
               Email == response.Email &&
               Surname == response.Surname;
    }
}