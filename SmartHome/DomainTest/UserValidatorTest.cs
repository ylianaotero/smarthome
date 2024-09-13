using Domain;

namespace DomainTest;

[TestClass]
public class UserValidatorTest
{
    [TestMethod]
    public void TestValidateName()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidateName("Juan");
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestValidateSurname()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidateSurname("Lopez");
        Assert.IsTrue(validated);
    }
}