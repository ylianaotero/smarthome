using Domain.Concrete;

namespace Model.Out;

public class PostAdministratorResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Surname { get; set; }
    
    public long Id { get; set; }

    public PostAdministratorResponse(User user, long id)
    {
        Name = user.Name;
        Email = user.Email;
        Surname = user.Surname;
        Id = id; 
    }

    public override bool Equals(object? obj)
    {
        return obj is PostAdministratorResponse response &&
               Name == response.Name &&
               Email == response.Email &&
               Surname == response.Surname;
    }
}