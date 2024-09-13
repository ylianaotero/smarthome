using System.Text.RegularExpressions;
using IDomain;

namespace Domain;

public class UserValidator : IUserValidator
{
    public bool ValidateName(string name)
    {
        return !IsNull(name);
    }
    
    public bool ValidateSurname(string surname)
    {
        return !IsNull(surname);
    }
    
    public bool ValidateEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        if (IsNull(email) || !Regex.IsMatch(email, pattern))
        {
            return false;
        }
        return true;
    }

    public bool ValidatePassword(string password)
    {
        if (IsNull(password))
        {
            return false;
        }
        if (!Regex.IsMatch(password, @"^(?=.*[#@$.,%])(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
        {
            return false;
        }
        return true;
    }

    private bool IsNull(string text)
    {
        return string.IsNullOrEmpty(text);
    }
}