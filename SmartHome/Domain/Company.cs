using IDomain;

namespace Domain;

public class Company : ICompany
{
    public string Name { get; set; }
    public string RUT { get; set; }
    public string LogoURL { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Company company &&
               Name == company.Name &&
               RUT == company.RUT &&
               LogoURL == company.LogoURL;
    }
}