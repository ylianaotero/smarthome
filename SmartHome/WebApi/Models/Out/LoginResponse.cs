using Domain;

namespace WebApi.Models.Out;

public class LoginResponse
{
    public Guid Token { get; set; }
    
    public LoginResponse(Session session)
    {
        Token = session.Id;
    }
}