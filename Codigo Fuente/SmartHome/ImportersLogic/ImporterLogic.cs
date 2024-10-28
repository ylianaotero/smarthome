using IBusinessLogic;

namespace ImportersLogic;

public class ImporterLogic : IImporter.IImporter
{
    private IDeviceService _deviceService; 
    
    public ImporterLogic(IDeviceService deviceService)
    {
        _deviceService = deviceService; 
    }

    public List<string> GetImplementationsNames(List<string> assemblyPaths)
    {
        throw new NotImplementedException();
    }

    public bool Import(string dllPath, string filePath, string type)
    {
        throw new NotImplementedException();
    }
}