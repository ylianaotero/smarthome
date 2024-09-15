using Domain.Exceptions.CompanyOwner;

namespace Domain;

public class CompanyOwner : Role
{
    private bool _hasACompleteCompany;
    private Company _company;

    public Company Company
    {
        get => _company;
        set
        {
            ValidateExistingCompany();
            _company = value;
        }
    }

    public CompanyOwner(Company company)
    {
        ValidateExistingCompany();
        Company = company;
        _hasACompleteCompany = true;
    }

    public CompanyOwner()
    {
        _hasACompleteCompany = false;
    }
    
    private void ValidateExistingCompany()
    {
        if (_hasACompleteCompany)
        {
            throw new ACompanyHasAlreadyBeenRegistredException("The owner has an existing company.");
        }
    }
}