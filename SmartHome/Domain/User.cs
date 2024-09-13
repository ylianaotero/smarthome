using IDomain;

namespace Domain;

public class User
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<IRole> Roles { get; set; }

    public User()
    {
        Roles = new List<IRole>();
    }
}