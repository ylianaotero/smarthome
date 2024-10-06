using Domain;
using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class UserValidatorTest
{
    private const string ValidName = "Juan";
    private const string ValidSurname = "Lopez";
    private const string ValidEmail = "juanLopez@gmail.com";
    private const string InvalidEmailNoAtSymbol = "juanLopezgmail.com";
    private const string InvalidEmailEmpty = "";
    private const string ValidPassword = "Password1@.";
    private const string InvalidPasswordEmpty = "";
    private const string InvalidPasswordNoSpecialChar = "password1";

    [TestMethod]
    public void TestValidateName()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidateName(ValidName);
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestValidateSurname()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidateSurname(ValidSurname);
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestValidateEmail()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidateEmail(ValidEmail);
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestCannotValidateEmailBecauseOfStructure()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidateEmail(InvalidEmailNoAtSymbol);
        Assert.IsFalse(validated);
    }
    
    [TestMethod]
    public void TestCannotValidateEmailBecauseItIsEmpty()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidateEmail(InvalidEmailEmpty);
        Assert.IsFalse(validated);
    }
    
    [TestMethod]
    public void TestValidatePassword()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidatePassword(ValidPassword);
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestCannotValidatePasswordBecauseItIsEmpty()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidatePassword(InvalidPasswordEmpty);
        Assert.IsFalse(validated);
    }
    
    [TestMethod]
    public void TestCannotValidatePasswordBecauseItHasCharactersThatCannotBeUsed()
    {
        UserValidator validator = new UserValidator();
        bool validated = validator.ValidatePassword(InvalidPasswordNoSpecialChar);
        Assert.IsFalse(validated);
    }
}
