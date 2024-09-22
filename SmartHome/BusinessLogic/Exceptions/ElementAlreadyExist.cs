namespace BusinessLogic.Exceptions;

public class ElementAlreadyExist : Exception
{
    public ElementAlreadyExist(string message) : base(message){}
}