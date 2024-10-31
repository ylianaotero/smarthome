namespace CustomExceptions;

public class CannotAccessItem : Exception
{
    public CannotAccessItem(string message) : base(message){}
}