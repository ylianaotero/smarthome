using Domain;

namespace TestWebApi.In;

public class LoginRequestTest
{
    private readonly string emailSample = "email@sample.com";
    private readonly string passwordSample = "passwordSample1@";
    private LoginRequest request;
    
    [TestInitialize]
    public void Init()
    {
        request = new LoginRequest()
        {
            Email = emailSample,
            Password = passwordSample
        };
    }
    
    [TestMethod]
    public void AssignsProperties()
    {
        Assert.AreEqual(request.Email, emailSample);
        Assert.AreEqual(request.Password, passwordSample);
    }
    
}