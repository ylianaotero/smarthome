using BusinessLogic;
using IBusinessLogic;
using Moq;

namespace TestService;

[TestClass]
public class ModelValidatorLogicTest
{
    private const int ExpectedResultCount = 2;
    
    [TestMethod]
    public void NewModelValidatorLogic()
    {
        ModelValidatorLogic modelValidatorLogic = new ModelValidatorLogic(); 
        
        Assert.IsNotNull(modelValidatorLogic);
    }
    
    [TestMethod]
    public void GetAllValidators_return_json()
    {
        ModelValidatorLogic modelValidatorLogic = new ModelValidatorLogic();

        var result = modelValidatorLogic.GetAllValidators();
        
        Assert.IsNotNull(result); 
        Assert.AreEqual(ExpectedResultCount, result.Count); 
    }
}