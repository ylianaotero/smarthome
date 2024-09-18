using WebApi.Out;

namespace TestWebApi.Out;

[TestClass]
public class LoginResponseTest
{
    private readonly Guid Guid = new Guid();
    private LoginResponse response;
    
    [TestInitialize]
    public void Init()
    {
        response = new LoginResponse()
        {
            Token = Guid
        };
    }
    
    [TestMethod]
    public void AssignsProperties()
    {
        Assert.AreEqual(response.Token, Guid);
    }
}