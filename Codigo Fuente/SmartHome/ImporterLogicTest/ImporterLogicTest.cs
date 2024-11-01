using System.Reflection;
using System.Reflection.Emit;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IImporter;
using ImportersLogic;
using Moq;

namespace ImporterLogicTest;

[TestClass]
public class ImporterLogicTest
{
    private Mock<IDeviceService> _mockDeviceService;
    
    private Mock<Assembly> _mockAssembly;
    
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
        Assert.AreEqual(1, result.Count); 
        
        foreach (var implementation in result)
        {
            Assert.AreEqual("JsonDeviceImporter.JsonDevicesImporter", implementation.ImplementationName); 
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
        
        List<Company> list = new List<Company>(); 
        
        list.Add(company);
        
        importerLogic.Import("directoryOfDll", "filePath", "json", list);
    }

    
    [TestMethod]
    public void Import_json_with_data()
    {
        string dllPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\DLLsImports\JsonImport\JsonDeviceImporter.dll"));
        
        string jsonPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\ExampleJson\example.json"));

        
        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(s => s.CreateDevice(It.IsAny<Device>())).Verifiable();
        
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);
        

        Company company = new Company(); 
        
        List<Company> list = new List<Company>(); 
        
        list.Add(company);
        
        bool res = importerLogic.Import( dllPath, jsonPath, "json", list);
        
        Assert.IsTrue(res);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void Import_type_not_found()
    {
        string dllPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\DLLsImports\JsonImport\JsonDeviceImporter.dll"));
        
        string jsonPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\ExampleJson\example.json"));

        
        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(s => s.CreateDevice(It.IsAny<Device>())).Verifiable();
        
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);
        

        Company company = new Company(); 
        
        List<Company> list = new List<Company>(); 
        
        list.Add(company);
        
        importerLogic.Import( dllPath, jsonPath, "html", list);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void Test_Import_type_dllFIle_not_found()
    {
        string dllPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\DLLsImports\JsonImport\noExiste.dll"));
        
        string jsonPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\ExampleJson\example.json"));

        
        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(s => s.CreateDevice(It.IsAny<Device>())).Verifiable();
        
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);
        

        Company company = new Company(); 
        
        List<Company> list = new List<Company>(); 
        
        list.Add(company);
        
        importerLogic.Import( dllPath, jsonPath, "json", list);
    }
    
    private void SetupMocks()
    {
        _mockDeviceService = new Mock<IDeviceService>();

    }
}