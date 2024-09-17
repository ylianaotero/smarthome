namespace Domain.Exceptions.GeneralExceptions;

public class InputNotValid : Exception
{
    public InputNotValid(string message) : base(message){}
}
