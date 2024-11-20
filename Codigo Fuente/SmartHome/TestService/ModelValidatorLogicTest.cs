using BusinessLogic;
using IBusinessLogic;
using Moq;

namespace TestService;

[TestClass]
public class ModelValidatorLogicTest
{
    private const string ImportersDirectory = "ActiveModels";
    private const string DllExtension = "dll";
    private const string DllSearchPattern = "*.dll";
    private const string DllDirectoryPath = @"..\..\..\..\DLLsImports";
    private const string JsonImportType = "json";
    private const string JsonImplementationName = "json";
    private const string FileNotFoundMessage = "File not found";
    private const string TypeOrDllNotFoundMessage = "Type or dll not found";
    private const string JsonImporterDllPath = @"..\..\..\..\DLLsImports\JsonImport\JsonDeviceImporter.dll";
    private const string ExampleJsonPath = @"..\..\..\..\ExampleJson\example.json";
    private const string ExampleNotValidPath = "notvalid";
    private const string NonExistentDllPath = @"..\..\..\..\DLLsImports\JsonImport\noExiste.dll";
    private const string ImportTypeJson = "json";
    private const string ImportTypeHtml = "html";
    private const string ImplementationName = "JsonDeviceImporter.JsonDevicesImporter";
    private const int ExpectedResultCount = 2;

    private Mock<IModelValidator> _mockModelValidator;

    [TestInitialize]
    public void TestInitialize()
    {
        SetupMocks();
    }
    
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
    
    /*
    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void GetAllValidators_throw_exception()
    {
        ModelValidatorLogic modelValidatorLogic = new ModelValidatorLogic();

        var result = modelValidatorLogic.GetAllValidators();
    }
    */
    
    private void SetupMocks()
    {
        _mockModelValidator = new Mock<IModelValidator>();
    }
}