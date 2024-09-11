using IDomain;

namespace Domain;

public class Administrator : IRole
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}