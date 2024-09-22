using Domain;

namespace TestDomain;

[TestClass]
public class SessionTest
{
    private const string ValidEmail = "juanlopez@gmail.com";
    private static readonly Guid Guid = new Guid();

    private User _user; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _user = new User();
        _user.Email = ValidEmail;
    }
    
    [TestMethod]
    public void TestNewSession()
    {
        Session session = new Session();
        
        session.User = _user;
        session.Id = Guid;
        
        Assert.IsTrue(
            ValidEmail == session.User.Email &&
            Guid == session.Id
        );

    }
    
}