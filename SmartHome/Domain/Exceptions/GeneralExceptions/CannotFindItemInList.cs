namespace Domain.Exceptions.GeneralExceptions;

public class CannotFindItemInList : Exception
{
    public CannotFindItemInList(string message) : base(message){}
}