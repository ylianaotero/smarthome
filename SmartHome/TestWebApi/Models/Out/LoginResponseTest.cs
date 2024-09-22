using WebApi.Out;

namespace TestWebApi.Models.Out;

[TestClass]
public class LoginResponseTest
{
    private readonly Guid _guid = new Guid();
    private LoginResponse _response;
    
    [TestInitialize]
    public void Init()
    {
        _response = new LoginResponse()
        {
            Token = _guid
        };
    }
    
    [TestMethod]
    public void AssignsProperties()
    {
        Assert.AreEqual(_response.Token, _guid);
    }
}