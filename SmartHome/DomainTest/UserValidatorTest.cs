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
}