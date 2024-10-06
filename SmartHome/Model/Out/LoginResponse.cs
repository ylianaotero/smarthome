using Domain.Concrete;

namespace Model.Out;

public class LoginResponse
{
    public Guid Token { get; set; }
    
    public LoginResponse(Session session)
    {
        Token = session.Id;
    }
}