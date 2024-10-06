using System.Text.RegularExpressions;
using Domain.Abstract;

namespace Domain.Concrete;

public class UserValidator : IUserValidator
{
    private const string EmailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
    private const string PasswordPattern = @"^(?=.*[#@$.,%])(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";

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
        if (IsNull(email) || !Regex.IsMatch(email, EmailPattern))
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
        
        if (!Regex.IsMatch(password, PasswordPattern))
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