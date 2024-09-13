namespace Domain.Exceptions.RoleExceptions;

public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string message) : base(message){}
}