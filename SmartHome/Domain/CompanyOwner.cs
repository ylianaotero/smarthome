using Domain.Exceptions.CompanyOwner;

namespace Domain;

public class CompanyOwner : Role
{
    public bool HasACompleteCompany { get; set; }
    private Company _company ;

    public Company Company
    {
        get => _company;
        set
        {
            ValidateExistingCompany();
            _company = value;
        }
    }
    public CompanyOwner()
    {
        HasACompleteCompany = false;
    }


    public CompanyOwner(Company company)
    {
        ValidateExistingCompany();
        Company = company;
        HasACompleteCompany = true;
    }
    
    private void ValidateExistingCompany()
    {
        if (HasACompleteCompany)
        {
            throw new ACompanyHasAlreadyBeenRegistredException("The owner has an existing company.");
        }
    }
}