using CustomExceptions;
using JsonDeviceImporter;

namespace ImporterTests;

[TestClass]
public class JsonImporterTest
{
    private const string ExampleJsonDirectoryPath = @"..\..\..\..\ExampleJson";
    private const string ValidJsonFileName = "example.json";
    private const string InvalidJsonFileName = "noExiste.json";

    [TestMethod]
    public void CreateObjectModel_ShouldReturnNonEmptyList()
    {
        JsonDevicesImporter importer = new JsonDevicesImporter();
        
        string directoryOfDll = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ExampleJsonDirectoryPath));
        var jsonFilePath = Path.Combine(directoryOfDll, ValidJsonFileName);
        
        var result = importer.CreateObjectModel(jsonFilePath); 
        
        Assert.IsNotNull(result); 
        Assert.IsTrue(result.Count > 0); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void Test_CreateObjectModel_FileNotFound()
    {
        JsonDevicesImporter importer = new JsonDevicesImporter();
        
        string directoryOfDll = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ExampleJsonDirectoryPath));
        var jsonFilePath = Path.Combine(directoryOfDll, InvalidJsonFileName);
        
        importer.CreateObjectModel(jsonFilePath); 
    }
}