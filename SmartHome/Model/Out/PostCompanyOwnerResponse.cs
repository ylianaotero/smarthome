using Domain.Abstract;
using Domain.Concrete;

namespace Model.Out;

public class PostCompanyOwnerResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Surname { get; set; }
    public string Role { get; set; }

    public PostCompanyOwnerResponse(User user)
    {
        Name = user.Name;
        Email = user.Email;
        Surname = user.Surname;
        Role = typeof(CompanyOwner).ToString();
    }

    public override bool Equals(object? obj)
    {
        return obj is PostCompanyOwnerResponse response &&
               Name == response.Name &&
               Email == response.Email &&
               Surname == response.Surname &&
                Role == response.Role;
    }
}