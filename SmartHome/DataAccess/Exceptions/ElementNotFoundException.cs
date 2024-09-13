namespace DataAccess.Exceptions;

public class ElementNotFoundException : Exception
{
    public ElementNotFoundException(string message) : base(message)
    {
    }
}