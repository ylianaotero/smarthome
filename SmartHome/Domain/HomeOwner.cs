using IDomain;

namespace Domain;

public class HomeOwner : IRole
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}