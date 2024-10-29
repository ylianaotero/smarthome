using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace TestWebApi.Controllers;

[TestClass]
public class ImportControllerTest
{
    private string _directoryPath; 
    private Mock<IImporter.IImporter> _mockImporter;

    private List<string> _listImporters; 

    private ImportController _importController; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        _mockImporter = new Mock<IImporter.IImporter>(MockBehavior.Strict);
        _directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..","..","..", "Imports", "JsonImporter");
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
    
    

    
    
}