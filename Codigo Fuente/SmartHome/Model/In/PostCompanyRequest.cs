using Domain.Concrete;

namespace Model.In;


public class PostCompanyRequest
{
    public string Name { get; set; }
    public string RUT { get; set; }
    public string LogoURL { get; set; }
    public long OwnerId { get; set; }
    public string? ValidationMethod { get; set; }

    public Company ToEntity()
    {
        return new Company()
        {
            Name = this.Name,
            RUT = this.RUT,
            LogoURL = this.LogoURL,
            ValidationMethod = this.ValidationMethod
        };
    }
}