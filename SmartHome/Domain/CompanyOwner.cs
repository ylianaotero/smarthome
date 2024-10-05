using CustomExceptions;

namespace Domain;

public class CompanyOwner : Role
{
    private const string MessageError = "The owner has an existing company."; 
    
    public override string Kind { get; set; }
    public bool HasACompleteCompany { get; set; }
    private Company _company ;

    public Company Company
    {
        get => _company;
        set
        {
            ValidateExistingCompany();
            _company = value;
            HasACompleteCompany = true;
        }
    }
    public CompanyOwner()
    {
        HasACompleteCompany = false;
        Kind = GetType().Name;
    }
    
    private void ValidateExistingCompany()
    {
        if (HasACompleteCompany)
        {
            throw new ElementAlreadyExist(MessageError);
        }
    }
}