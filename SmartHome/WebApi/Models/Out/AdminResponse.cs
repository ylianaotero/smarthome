using Domain;

namespace WebApi.Models.Out;

public class AdminResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    
    public string Surname { get; set; }

    public AdminResponse(User user)
    {
        Name = user.Name;
        Email = user.Email;
        Surname = user.Surname;
    }
}