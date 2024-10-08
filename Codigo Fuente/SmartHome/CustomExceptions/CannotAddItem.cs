namespace CustomExceptions;

public class CannotAddItem : Exception
{
    public CannotAddItem(string message) : base(message){}
}