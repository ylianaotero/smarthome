using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class UserValidatorTest
{
    private UserValidator _userValidator;
    
    private const string ValidName = "Juan";
    private const string ValidSurname = "Lopez";
    private const string ValidEmail = "juanLopez@gmail.com";
    private const string InvalidEmailNoAtSymbol = "juanLopezgmail.com";
    private const string InvalidEmailEmpty = "";
    private const string ValidPassword = "Password1@.";
    private const string InvalidPasswordEmpty = "";
    private const string InvalidPasswordNoSpecialChar = "password1";
    
    [TestInitialize]
    public void Initialize()
    {
        _userValidator = new UserValidator();
    }

    [TestMethod]
    public void TestValidateName()
    {
        bool validated = _userValidator.ValidateName(ValidName);
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestValidateSurname()
    {
        bool validated = _userValidator.ValidateSurname(ValidSurname);
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestValidateEmail()
    {
        bool validated = _userValidator.ValidateEmail(ValidEmail);
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestCannotValidateEmailBecauseOfStructure()
    {
        bool validated = _userValidator.ValidateEmail(InvalidEmailNoAtSymbol);
        Assert.IsFalse(validated);
    }
    
    [TestMethod]
    public void TestCannotValidateEmailBecauseItIsEmpty()
    {
        bool validated = _userValidator.ValidateEmail(InvalidEmailEmpty);
        Assert.IsFalse(validated);
    }
    
    [TestMethod]
    public void TestValidatePassword()
    {
        bool validated = _userValidator.ValidatePassword(ValidPassword);
        Assert.IsTrue(validated);
    }
    
    [TestMethod]
    public void TestCannotValidatePasswordBecauseItIsEmpty()
    {
        bool validated = _userValidator.ValidatePassword(InvalidPasswordEmpty);
        Assert.IsFalse(validated);
    }
    
    [TestMethod]
    public void TestCannotValidatePasswordBecauseItHasCharactersThatCannotBeUsed()
    {
        bool validated = _userValidator.ValidatePassword(InvalidPasswordNoSpecialChar);
        Assert.IsFalse(validated);
    }
}
