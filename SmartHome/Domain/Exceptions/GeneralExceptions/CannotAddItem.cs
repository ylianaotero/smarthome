namespace Domain.Exceptions.GeneralExceptions;

public class CannotAddItem : Exception
{
    public CannotAddItem(string message) : base(message){}
}