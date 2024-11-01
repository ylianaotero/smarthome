using JsonDeviceImporter;

namespace ImporterTests;

[TestClass]
public class JsonImporterTest
{
    [TestMethod]
    public void CreateObjectModel_ShouldReturnNonEmptyList()
    {
        JsonDevicesImporter importer = new JsonDevicesImporter();
        
        string directoryOfDll = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\ExampleJson"));
        var jsonFilePath = Path.Combine(directoryOfDll, "example.json");
        
        var result = importer.CreateObjectModel(jsonFilePath); 
        
        Assert.IsNotNull(result); 
        Assert.IsTrue(result.Count > 0); 
    }
    
    

}