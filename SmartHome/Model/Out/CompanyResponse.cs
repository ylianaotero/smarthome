using Domain.Concrete;

namespace Model.Out;

public class CompanyResponse
{
    public string Name { get; set;  }
    public string RUT { get; set;  }
    public string? LogoURL { get; set;  }
    
    public CompanyResponse (Company company)
    {
        Name = company.Name;
        RUT = company.RUT;
        LogoURL = company.LogoURL ?? string.Empty;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is CompanyResponse response &&
               Name == response.Name &&
               RUT == response.RUT &&
               LogoURL == response.LogoURL;
    }
}