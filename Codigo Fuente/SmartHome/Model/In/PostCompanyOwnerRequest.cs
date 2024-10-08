using Domain.Abstract;
using Domain.Concrete;

namespace Model.In;

public class PostCompanyOwnerRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Surname { get; set; }

    public User ToEntity()
    {
        CompanyOwner companyOwner = new CompanyOwner();
        
        return new User
        {
            Name = Name,
            Email = Email,
            Password = Password,
            Surname = Surname,
            Roles = new List<Role>{companyOwner}
        };
    }
}