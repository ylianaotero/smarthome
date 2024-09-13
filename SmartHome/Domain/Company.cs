using System.ComponentModel.DataAnnotations;
using IDomain;

namespace Domain;

public class Company
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public string RUT { get; set; }
    public string LogoURL { get; set; }
    
    public override bool Equals(object? obj)
    {
        return obj is Company company &&
               Id == company.Id &&
               Name == company.Name &&
               RUT == company.RUT &&
               LogoURL == company.LogoURL;
    }
}