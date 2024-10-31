using JsonDeviceImporter;

namespace ImporterTests;

[TestClass]
public class JsonImporterTest
{
    [TestMethod]
    public void CreateObjectModel_ShouldReturnNonEmptyList()
    {
        JsonDevicesImporter importer = new JsonDevicesImporter();
    
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "DeviceImporter");
        var jsonFilePath = Path.Combine(directoryPath, "devices-to-import.json");
        
        var result = importer.CreateObjectModel(jsonFilePath); 
        
        Assert.IsNotNull(result); 
        Assert.IsTrue(result.Count > 0); 
    }
    
    

}