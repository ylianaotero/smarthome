using System.Text.RegularExpressions;
using Domain.Concrete;

namespace ValidatorLetter;
public class ValidatorLetter : IModelValidator
{
    public bool EsValido(Modelo modelo)
    {
        string pattern = @"^[a-zA-Z]{6}$";
        return Regex.IsMatch(modelo.get_Value(), pattern);
    }
}
