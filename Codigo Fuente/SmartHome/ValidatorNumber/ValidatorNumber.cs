using System.Text.RegularExpressions;
using Domain.Concrete;

namespace ValidatorNumber;

public class ValidatorNumber : IModelValidator
{
    public bool EsValido(Modelo modelo)
    {
        if (modelo.get_Value().Length != 6)
            return false;
        
        string pattern = @"^[a-zA-Z]{3}\d{3}$";
        return Regex.IsMatch(modelo.get_Value(), pattern);
    }
}

