using Domain.Concrete;

namespace Model.Out;

public class GetCompanyResponse
{
    public long Id { get; set; }
    public string Owner { get; set; }
    public string OwnerEmail { get; set; }
    public string Name { get; set;  }
    public string RUT { get; set;  }
    public string? LogoURL { get; set;  }
    
    public GetCompanyResponse (Company company)
    {
        Name = company.Name;
        RUT = company.RUT;
        LogoURL = company.LogoURL ?? string.Empty;
        Owner = company.Owner.Name + " " + company.Owner.Surname;
        OwnerEmail = company.Owner.Email;
        Id = company.Id;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetCompanyResponse response &&
               Name == response.Name &&
               RUT == response.RUT &&
               LogoURL == response.LogoURL &&
               Owner == response.Owner &&
                OwnerEmail == response.OwnerEmail &&
               Id == response.Id;
    }
}