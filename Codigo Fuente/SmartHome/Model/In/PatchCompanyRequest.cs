using Domain.Concrete;

namespace Model.In;

public class PatchCompanyRequest
{
    public string? ValidationMethod { get; set; }
    
    public Company ToEntity()
    {
        return new Company()
        {
            ValidationMethod = this.ValidationMethod
        };
    }
}