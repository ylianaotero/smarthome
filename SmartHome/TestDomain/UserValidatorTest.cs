using Domain;

namespace TestDomain;

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
    
    [TestMethod]
    public void TestValidateEmail()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidateEmail("juanLopez@gmail.com");
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestCannotValidateEmailBecauseOfStructure()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidateEmail("juanLopezgmail.com");
        Assert.IsFalse(validated);
    }
    
    [TestMethod]
    public void TestCannotValidateEmailBecauseItIsEmpty()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidateEmail("");
        Assert.IsFalse(validated);
    }
    
    [TestMethod]
    public void TestValidatePassword()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidatePassword("Password1@.");
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestCannotValidatePasswordBecauseItIsEmpty()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidatePassword("");
        Assert.IsFalse(validated);
    }
    
    [TestMethod]
    public void TestCannotValidatePasswordBecauseItHasCharactersThatCannotBeUsed()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidatePassword("password1");
        Assert.IsFalse(validated);
    }
}