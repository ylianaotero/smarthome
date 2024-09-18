namespace Domain.Exceptions.GeneralExceptions;

public class ElementNotFound : Exception
{
    public ElementNotFound(string message) : base(message){}
}