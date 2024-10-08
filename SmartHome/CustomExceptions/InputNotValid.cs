namespace CustomExceptions;

public class InputNotValid : Exception
{
    public InputNotValid(string message) : base(message){}
}
