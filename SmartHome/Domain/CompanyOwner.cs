using IDomain;

namespace Domain;

public class CompanyOwner : IRole
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}