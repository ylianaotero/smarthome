namespace Domain.DomainExceptions.RoleException;

public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string message) : base(message){}
}