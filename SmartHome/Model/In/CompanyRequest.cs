using Domain;
using IDataAccess;

namespace Model.In;


public class CompanyRequest
{
    public string Name { get; set; }
    public string RUT { get; set; }
    public string LogoURL { get; set; }
    public long OwnerId { get; set; }

    public Company ToEntity()
    {
        return new Company()
        {
            Name = this.Name,
            RUT = this.RUT,
            LogoURL = this.LogoURL
        };
    }
}