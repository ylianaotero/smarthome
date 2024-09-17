namespace Domain.Exceptions.GeneralExceptions;

public class ElementAlreadyExist : Exception
{
    public ElementAlreadyExist(string message) : base(message){}
}