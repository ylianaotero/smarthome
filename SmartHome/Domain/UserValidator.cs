using IDomain;

namespace Domain;

public class UserValidator
{
    public bool ValidateName(string name)
    {
        return !IsNull(name);
    }
    
    public bool ValidateSurname(string surname)
    {
        return !IsNull(surname);
    }

    private bool IsNull(string text)
    {
        return string.IsNullOrEmpty(text);
    }
}