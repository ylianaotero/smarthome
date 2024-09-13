using System.Text.RegularExpressions;

namespace IDomain;

public interface IUserValidator
{
    public bool ValidateName(String name);
    public bool ValidateSurname(String surname);
    public bool ValidateEmail(String email);
    public bool ValidatePassword(String password);

}