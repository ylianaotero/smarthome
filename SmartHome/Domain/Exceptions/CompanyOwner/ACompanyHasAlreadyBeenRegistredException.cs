namespace Domain.Exceptions.CompanyOwner;

public class ACompanyHasAlreadyBeenRegistredException : Exception
{
    public ACompanyHasAlreadyBeenRegistredException(string message) : base(message){}

}