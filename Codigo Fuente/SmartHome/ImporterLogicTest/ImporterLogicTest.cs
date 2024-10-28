using IBusinessLogic;
using ImportersLogic;
using Moq;

namespace ImporterLogicTest;

[TestClass]
public class ImporterLogicTest
{
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
    
    private void SetupMocks()
    {
        _mockDeviceService = new Mock<IDeviceService>();
    }
}