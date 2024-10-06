using Domain.Concrete;

namespace Model.Out;

public class GetCompanyResponse
{
    public string Name { get; set;  }
    public string RUT { get; set;  }
    public string? LogoURL { get; set;  }
    
    public GetCompanyResponse (Company company)
    {
        Name = company.Name;
        RUT = company.RUT;
        LogoURL = company.LogoURL ?? string.Empty;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetCompanyResponse response &&
               Name == response.Name &&
               RUT == response.RUT &&
               LogoURL == response.LogoURL;
    }
}