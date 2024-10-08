using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class SessionTest
{
    private User _user;
    private Session _session;
    
    private static readonly Guid Guid = new Guid();
    private const string ValidEmail = "juanlopez@gmail.com";
    
    [TestInitialize]
    public void TestInitialize()
    {
        _user = new User();
        _session = new Session();
        _user.Email = ValidEmail;
    }
    
    [TestMethod]
    public void TestNewSession()
    {
        _session.User = _user;
        _session.Id = Guid;
        
        Assert.IsTrue(
            ValidEmail == _session.User.Email &&
            Guid == _session.Id
        );
    }
    
}