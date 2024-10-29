using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace TestWebApi.Controllers;

[TestClass]
public class ImportControllerTest
{
    private const int NotFoundStatusCode = 404;
    
    private string _directoryPath; 
    private string _dllFile; 
    private string _jsonFile; 
    
    private Mock<IImporter.IImporter> _mockImporter;

    private List<string> _listImporters; 

    private ImportController _importController; 
    
    private const int OKStatusCode = 200;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _mockImporter = new Mock<IImporter.IImporter>(MockBehavior.Strict);
        _directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..","..","..", "Imports", "JsonImporter");
        _dllFile = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Imports", "JsonImporter", "JsonDeviceImporter.dll");
        _jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "DeviceImporter", "devices-to-import.json");
        _listImporters = new List<string>() { "JsonDeviceImporter.JsonDevicesImporter"};
    }
    
    [TestMethod]
    public void Test_GetImportersName_Okay()
    {
        _mockImporter
            .Setup(x => x.GetImplementationsNames(_directoryPath))
            .Returns(_listImporters);
        
        _importController = new ImportController(_mockImporter.Object);
        
        ObjectResult result = _importController.GetNames(_directoryPath) as OkObjectResult;
        List<string> response = result.Value as List<string>;
        
        _mockImporter.Verify();
        
        Assert.AreEqual(_listImporters, response);
    }
    
    [TestMethod]
    public void Test_GetImportersName_NotFound()
    {
        _mockImporter
            .Setup(x => x.GetImplementationsNames(_directoryPath))
            .Throws(new Exception());
        
        _importController = new ImportController(_mockImporter.Object);
        
        ObjectResult result = _importController.GetNames(_directoryPath) as ObjectResult;
        
        _mockImporter.Verify();
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void Test_Import_Okay()
    {
        _mockImporter
            .Setup(x => x.Import(_dllFile,_jsonFile,"json"))
            .Returns(true);
        
        _importController = new ImportController(_mockImporter.Object);
        
        ObjectResult result = _importController.Import(_dllFile,_jsonFile, "json") as OkObjectResult;
        
        _mockImporter.Verify();
        
        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    
    

    
    
}