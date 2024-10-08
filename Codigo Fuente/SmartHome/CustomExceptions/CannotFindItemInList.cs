namespace CustomExceptions;

public class CannotFindItemInList : Exception
{
    public CannotFindItemInList(string message) : base(message){}
}