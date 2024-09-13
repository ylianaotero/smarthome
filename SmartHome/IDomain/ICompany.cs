using System.ComponentModel.DataAnnotations;

namespace IDomain;

public interface ICompany
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public string RUT { get; set; }
    public string LogoURL { get; set; }
}