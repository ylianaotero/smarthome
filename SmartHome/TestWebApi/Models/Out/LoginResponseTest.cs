using Domain;
using WebApi.Models.Out;

namespace TestWebApi.Models.Out;

[TestClass]
public class LoginResponseTest
{
    private readonly Guid _guid = new Guid();
    private LoginResponse _response;
    
    [TestInitialize]
    public void Init()
    {
        Session session = new Session()
        {
            Id = _guid
        };
        
        _response = new LoginResponse(session);
    }
    
    [TestMethod]
    public void AssignsProperties()
    {
        Assert.AreEqual(_response.Token, _guid);
    }
}