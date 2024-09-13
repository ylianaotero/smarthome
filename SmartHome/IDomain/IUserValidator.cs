using System.Text.RegularExpressions;

namespace IDomain;

public interface IUserValidator
{
    public bool validateName(String name);
    public bool validateSurname(String surname);
    public bool validateEmail(String email);
    public bool validatePassword(String password);

}