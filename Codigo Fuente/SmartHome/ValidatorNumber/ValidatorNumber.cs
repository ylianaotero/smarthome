using System.Text.RegularExpressions;

namespace ValidatorNumber;

public class ValidatorNumber : IModelValidator
{
    public bool IsValid(string model)
    {
        if (model.Length != 6)
            return false;
        
        string pattern = @"^\d{3}[a-zA-Z]{3}$";
        return Regex.IsMatch(model, pattern);
    }
}