using CustomExceptions;
using JsonDeviceImporter;

namespace ImporterTests;

[TestClass]
public class JsonImporterTest
{
    private const string ValidJsonFileName = "example.json";
    private const string InvalidJsonFileName = "noExiste.json";

    [TestMethod]
    public void CreateObjectModel_ShouldReturnNonEmptyList()
    {
        JsonDevicesImporter importer = new JsonDevicesImporter();

        var jsonFilePath = GetExampleJsonPath(ValidJsonFileName);

        var result = importer.CreateObjectModel(jsonFilePath);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Count > 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void Test_CreateObjectModel_FileNotFound()
    {
        JsonDevicesImporter importer = new JsonDevicesImporter();

        var jsonFilePath = GetExampleJsonPath(InvalidJsonFileName);

        importer.CreateObjectModel(jsonFilePath);
    }
    
    private string GetExampleJsonPath(string fileName)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string relativePath = Path.Combine("..", "..", "..", "..", "ExampleJson", fileName);
        return Path.GetFullPath(Path.Combine(basePath, relativePath));
    }
}