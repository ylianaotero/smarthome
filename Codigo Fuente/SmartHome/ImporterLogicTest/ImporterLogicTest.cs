using System.Reflection;
using System.Reflection.Emit;
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
    public void GetImplementationsNames()
    {
        var mockDeviceService = new Mock<IDeviceService>();
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);
        
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..","..","..", "Imports");

        var result = importerLogic.GetImplementationsNamesAndPath(directoryPath);
        
        Assert.IsNotNull(result); 
        Assert.AreEqual(0, result.Count); 
    }
    
    [TestMethod]
    public void GetImplementationsNames_return_json()
    {
        var mockDeviceService = new Mock<IDeviceService>();
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);
        
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..","..","..", "Imports", "JsonImporter");

        var result = importerLogic.GetImplementationsNamesAndPath(directoryPath);
        
        Assert.IsNotNull(result); 
        Assert.AreEqual(1, result.Count); 
        
        foreach (var implementation in result)
        {
            Assert.AreEqual("JsonDeviceImporter.JsonDevicesImporter", implementation.ImplementationName); 
        }
    }
    
    
    [TestMethod]
    public void Import_json()
    {
        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(s => s.CreateDevice(It.IsAny<Device>())).Verifiable();
        
        ImporterLogic importerLogic = new ImporterLogic(mockDeviceService.Object);
        
        var dllPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Imports", "JsonImporter", "JsonDeviceImporter.dll");
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "DeviceImporter", "devices-to-import.json");

        Company company = new Company(); 

        CompanyOwner companyOwner = new CompanyOwner() {Company = company}; 
        
        List<CompanyOwner> list = new List<CompanyOwner>(); 
        
        list.Add(companyOwner);
        
        var result = importerLogic.Import(dllPath, filePath, "json",list );
        
        Assert.IsTrue(result);
        mockDeviceService.Verify(s => s.CreateDevice(It.IsAny<Device>()), Times.AtLeastOnce);
    }

    
    private void SetupMocks()
    {
        _mockDeviceService = new Mock<IDeviceService>();

    }
}