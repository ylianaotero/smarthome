using IDomain;

namespace Domain;

public class UserValidator
{
    public bool ValidateName(string name)
    {
        return !IsNull(name);
    }

    private bool IsNull(string text)
    {
        return string.IsNullOrEmpty(text);
    }
}