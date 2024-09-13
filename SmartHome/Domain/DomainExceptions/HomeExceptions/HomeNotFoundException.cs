namespace Domain.DomainExceptions;

public class HomeNotFoundException : Exception
{
    public HomeNotFoundException(string message) : base(message){}
}