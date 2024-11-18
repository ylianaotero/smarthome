using System.Reflection;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using ImportersLogic;
using Moq;

namespace ImporterLogicTest;

[TestClass]
public class ImporterLogicTest
{
    private const string JsonImporterDllPath = @"..\..\..\..\DLLsImports\JsonImport\JsonDeviceImporter.dll";
    private const string ExampleJsonPath = @"..\..\..\..\ExampleJson\example.json";
    private const string ExampleNotValidPath = "notvalid";
    private const string NonExistentDllPath = @"..\..\..\..\DLLsImports\JsonImport\noExiste.dll";
    private const string ImportTypeJson = "json";
    private const string ImportTypeHtml = "html";
    private const string ImplementationName = "JsonDeviceImporter.JsonDevicesImporter";
    private const int ExpectedResultCount = 1;

    private Mock<IDeviceService> _mockDeviceService;

    [TestInitialize]
    public void TestInitialize()
    {
        SetupMocks();
    }
    
    [TestMethod]
    public void NewImporterLogic()
    {
        ImporterLogic importerLogic = new ImporterLogic(_mockDeviceService.Object); 
        
        Assert.IsNotNull(importerLogic);
    }
    
    [TestMethod]
    public void GetImplementationsNames_return_json()
    {
        var mockDeviceService = new Mock<IDeviceService>();
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);

        var result = importerLogic.GetImplementationsNamesAndPath();
        
        Assert.IsNotNull(result); 
        Assert.AreEqual(ExpectedResultCount, result.Count); 
        
        foreach (var implementation in result)
        {
            Assert.AreEqual(ImplementationName, implementation.ImplementationName); 
        }
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void Import_json()
    {
        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(s => s.CreateDevice(It.IsAny<Device>())).Verifiable();
        
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);

        Company company = new Company(); 
        
        List<Company> list = new List<Company> { company };
        
        importerLogic.Import(ExampleNotValidPath, ExampleNotValidPath, ImportTypeJson, list);
    }

    [TestMethod]
    public void Import_json_with_data()
    {
        string dllPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), JsonImporterDllPath));
        string jsonPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ExampleJsonPath));

        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(s => s.CreateDevice(It.IsAny<Device>())).Verifiable();
        
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);

        Company company = new Company(); 
        
        List<Company> list = new List<Company> { company };
        
        bool res = importerLogic.Import(dllPath, jsonPath, ImportTypeJson, list);
        
        Assert.IsTrue(res);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void Import_type_not_found()
    {
        string dllPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), JsonImporterDllPath));
        string jsonPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ExampleJsonPath));

        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(s => s.CreateDevice(It.IsAny<Device>())).Verifiable();
        
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);

        Company company = new Company(); 
        
        List<Company> list = new List<Company> { company };
        
        importerLogic.Import(dllPath, jsonPath, ImportTypeHtml, list);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void Test_Import_type_dllFile_not_found()
    {
        string dllPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), NonExistentDllPath));
        string jsonPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ExampleJsonPath));

        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(s => s.CreateDevice(It.IsAny<Device>())).Verifiable();
        
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);

        Company company = new Company(); 
        
        List<Company> list = new List<Company> { company };
        
        importerLogic.Import(dllPath, jsonPath, ImportTypeJson, list);
    }
    
    private void SetupMocks()
    {
        _mockDeviceService = new Mock<IDeviceService>();
    }
}
