using Domain;

namespace DomainTest;

[TestClass]
public class SessionTest
{
    private const string ValidEmail = "juanlopez@gmail.com";

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
        
        Assert.AreEqual(ValidEmail, session.User.Email);
    }
    
}