using Domain;

namespace WebApi.Models.In;

public class CompanyRequest
{
    public string? Name { get; set; }
    public string? RUT { get; set; }
    public string? LogoURL { get; set; }

    public Company ToEntity()
    {
        return new Company()
        {
            Name = this.Name,
            RUT = this.RUT,
            LogoURL = this.LogoURL
        };
    }
    
    public Func<Company, bool> ToFilter() 
    {
        return company => (string.IsNullOrEmpty(this.Name) || company.Name == this.Name) &&
                           (string.IsNullOrEmpty(this.RUT) || company.RUT == this.RUT) &&
                           (string.IsNullOrEmpty(this.LogoURL) || company.LogoURL == this.LogoURL);
    }
}