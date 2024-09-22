namespace BusinessLogic.Exceptions;

public class ElementNotFound : Exception
{
    public ElementNotFound(string message) : base(message){}
}