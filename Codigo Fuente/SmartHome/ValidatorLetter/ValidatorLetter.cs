using System.Text.RegularExpressions;

namespace ValidatorLetter;

public class ValidatorLetter : IModelValidator
{
    public bool IsValid(string input)
    {
        string pattern = @"^[a-zA-Z]{6}$";
        return Regex.IsMatch(input, pattern);
    }
}