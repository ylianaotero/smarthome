namespace Domain.Exceptions.RoleException;

public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string message) : base(message){}
}