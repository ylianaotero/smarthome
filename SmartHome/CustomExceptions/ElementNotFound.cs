namespace CustomExceptions;

public class ElementNotFound : Exception
{
    public ElementNotFound(string message) : base(message){}
}