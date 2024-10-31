using Domain.Concrete;
using IBusinessLogic;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
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
    
    private Mock<ISessionService> _mockSessionService;

    private List<ImportResponse> _listImporters; 

    private ImportController _importController; 
    
    private const int OKStatusCode = 200;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _mockImporter = new Mock<IImporter.IImporter>(MockBehavior.Strict);
        _directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..","..","..", "Imports", "JsonImporter");
        _dllFile = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Imports", "JsonImporter", "JsonDeviceImporter.dll");
        _jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "DeviceImporter", "devices-to-import.json");
        _listImporters = new List<ImportResponse>
        {
            new ImportResponse()
            {
                ImplementationName = "JsonDeviceImporter.JsonDevicesImporter",
                AssemblyLocation = "ruta/del/ensamblado/JsonDeviceImporter.dll"
            }
        };
        _mockSessionService = new Mock<ISessionService>(MockBehavior.Strict); 
    }
    
    [TestMethod]
    public void Test_GetImportersName_Okay()
    {
        _mockImporter
            .Setup(x => x.GetImplementationsNamesAndPath(_directoryPath))
            .Returns(_listImporters);
        
        _importController = new ImportController(_mockImporter.Object,_mockSessionService.Object);
        
        ObjectResult result = _importController.GetNames(_directoryPath) as OkObjectResult;
        List<ImportResponse> response = result.Value as List<ImportResponse>;
        
        _mockImporter.Verify();
        
        Assert.AreEqual(_listImporters, response);
    }
    
    [TestMethod]
    public void Test_GetImportersName_NotFound()
    {
        _mockImporter
            .Setup(x => x.GetImplementationsNamesAndPath(_directoryPath))
            .Throws(new Exception());
        
        _importController = new ImportController(_mockImporter.Object,_mockSessionService.Object);
        
        ObjectResult result = _importController.GetNames(_directoryPath) as ObjectResult;
        
        _mockImporter.Verify();
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void Test_Import_Okay()
    {
        _mockImporter
            .Setup(x => x.Import(_dllFile,_jsonFile,"json", new List<CompanyOwner>()))
            .Returns(true);
        
        _importController = new ImportController(_mockImporter.Object,_mockSessionService.Object);

        ImportRequest importRequest = new ImportRequest()
        {
            DllPath = _dllFile,
            FilePath = _jsonFile,
            Type = "json"
        }; 
        
        OkObjectResult result = _importController.Import(importRequest) as OkObjectResult;
        
        _mockImporter.Verify();
        
        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    
    

    
    
}