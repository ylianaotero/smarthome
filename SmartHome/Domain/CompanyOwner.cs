namespace Domain;

public class CompanyOwner : Role
{
    public Company Company { get; set; }
    
    public CompanyOwner(Company company)
    {
        Company = company;
    }
}