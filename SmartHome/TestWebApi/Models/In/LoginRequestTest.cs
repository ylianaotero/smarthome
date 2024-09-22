using Microsoft.AspNetCore.Identity.Data;

namespace TestWebApi.Models.In;

public class LoginRequestTest
{
    private const string EmailSample = "email@sample.com";
    private const string PasswordSample = "passwordSample1@";
    private LoginRequest _request;
    
    [TestInitialize]
    public void Init()
    {
        _request = new LoginRequest()
        {
            Email = EmailSample,
            Password = PasswordSample
        };
    }
    
    [TestMethod]
    public void AssignsProperties()
    {
        Assert.IsTrue(
            _request.Email == EmailSample &&
            _request.Password == PasswordSample
        );

    }
    
}