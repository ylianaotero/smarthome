using BusinessLogic;
using CustomExceptions;
using Domain.Concrete;
using IBusinessLogic;
using Microsoft.AspNetCore.Http;
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
    
    private string _dllFile; 
    private string _jsonFile; 
    
    private const string ElementNotFoundMessage = "Element not found";
    
    private Mock<IImporter.IImporter> _mockImporter;
    
    private Mock<ISessionService> _mockSessionService;
    
    private Mock<ICompanyService> _mockCompanyService;

    private List<ImportResponse> _listImporters; 

    private ImportController _importController; 
    
    private const int OKStatusCode = 200;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _mockImporter = new Mock<IImporter.IImporter>(MockBehavior.Strict);
        _mockCompanyService = new Mock<ICompanyService>(MockBehavior.Strict);
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
            .Setup(x => x.GetImplementationsNamesAndPath())
            .Returns(_listImporters);
        
        _importController = new ImportController(_mockImporter.Object,_mockSessionService.Object, _mockCompanyService.Object);
        
        ObjectResult result = _importController.GetNames() as OkObjectResult;
        List<ImportResponse> response = result.Value as List<ImportResponse>;
        
        _mockImporter.VerifyAll();
        
        Assert.AreEqual(_listImporters, response);
    }
    
    [TestMethod]
    public void Test_GetImportersName_NotFound()
    {
        _mockImporter
            .Setup(x => x.GetImplementationsNamesAndPath())
            .Throws(new Exception());
        
        _importController = new ImportController(_mockImporter.Object,_mockSessionService.Object , _mockCompanyService.Object);
        
        ObjectResult result = _importController.GetNames() as ObjectResult;
        
        _mockImporter.VerifyAll();
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void Test_Import_Okay()
    {
        var httpContext = new DefaultHttpContext();
        string validGuid = Guid.NewGuid().ToString(); 
        httpContext.Request.Headers["Authorization"] = validGuid;
        
        
        _mockSessionService.Setup(x => x.GetUserId(It.IsAny<Guid>())).Returns(1);

        _mockCompanyService.Setup(x => x.GetCompaniesOfOwners(1)).Returns(new List<Company>()); 
            

        _mockImporter
            .Setup(x => x.Import(_dllFile, _jsonFile, "json",new List<Company>() ))
            .Returns(true);

        _importController = new ImportController(_mockImporter.Object, _mockSessionService.Object, _mockCompanyService.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext 
            }
        };

        ImportRequest importRequest = new ImportRequest()
        {
            DllPath = _dllFile,
            FilePath = _jsonFile,
            Type = "json"
        };

        ObjectResult result = _importController.Import(importRequest) as ObjectResult;

        _mockImporter.VerifyAll();
        
        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    
    [TestMethod]
    public void Test_Import_notFound()
    {
        var httpContext = new DefaultHttpContext();
        string validGuid = Guid.NewGuid().ToString(); 
        httpContext.Request.Headers["Authorization"] = validGuid;
        
        
        _mockSessionService.Setup(x => x.GetUserId(It.IsAny<Guid>())).Throws(new CannotFindItemInList(ElementNotFoundMessage ));

        _mockCompanyService.Setup(x => x.GetCompaniesOfOwners(1)).Returns(new List<Company>()); 
            

        _mockImporter
            .Setup(x => x.Import(_dllFile, _jsonFile, "json",new List<Company>() ))
            .Returns(true);

        _importController = new ImportController(_mockImporter.Object, _mockSessionService.Object, _mockCompanyService.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext 
            }
        };

        ImportRequest importRequest = new ImportRequest()
        {
            DllPath = _dllFile,
            FilePath = _jsonFile,
            Type = "json"
        };

        ObjectResult result = _importController.Import(importRequest) as ObjectResult;

        _mockImporter.Verify();
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void Test_Import_PathNotFound()
    {
        var httpContext = new DefaultHttpContext();
        string validGuid = Guid.NewGuid().ToString(); 
        httpContext.Request.Headers["Authorization"] = validGuid;
        
        
        _mockSessionService.Setup(x => x.GetUserId(It.IsAny<Guid>())).Returns(1);

        _mockCompanyService.Setup(x => x.GetCompaniesOfOwners(1)).Returns(new List<Company>());


        _mockImporter
            .Setup(x => x.Import(_dllFile, _jsonFile, "json", new List<Company>()))
            .Returns(false); 

        _importController = new ImportController(_mockImporter.Object, _mockSessionService.Object, _mockCompanyService.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext 
            }
        };

        ImportRequest importRequest = new ImportRequest()
        {
            DllPath = _dllFile,
            FilePath = _jsonFile,
            Type = "json"
        };

        ObjectResult result = _importController.Import(importRequest) as ObjectResult;

        _mockImporter.VerifyAll();
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestPostImporterOk()
    {
        _mockImporter.Setup(x => x.MoveDllFile(It.IsAny<string>())).Returns(true);

        _importController = new ImportController(_mockImporter.Object, _mockSessionService.Object, _mockCompanyService.Object) {};

        _importController.PostImporter(new PostImportRequest() { Path = _dllFile });

        _mockImporter.VerifyAll();
    }
    
    [TestMethod]
    public void TestPostImporterNotFound()
    {
        _mockImporter.Setup(x => x.MoveDllFile(_dllFile)).Returns(false);
        
        _importController = new ImportController(_mockImporter.Object, _mockSessionService.Object, _mockCompanyService.Object);

        _importController.PostImporter(new PostImportRequest() { Path = _dllFile });
        
        _mockImporter.VerifyAll();
    }
    
    [TestMethod]
    public void TestPostImportRequest()
    {
        PostImportRequest postImportRequest = new PostImportRequest()
        {
            Path = _dllFile
        };
        
        Assert.AreEqual(_dllFile, postImportRequest.Path);
    }
}