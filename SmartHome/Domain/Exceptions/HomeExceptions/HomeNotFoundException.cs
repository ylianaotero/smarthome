namespace Domain.Exceptions;

public class HomeNotFoundException : Exception
{
    public HomeNotFoundException(string message) : base(message){}
}