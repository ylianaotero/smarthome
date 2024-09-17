namespace Domain.Exceptions.HomeExceptions;

public class HomeNotFoundException : Exception
{
    public HomeNotFoundException(string message) : base(message){}
}