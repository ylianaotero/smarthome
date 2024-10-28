namespace IImporter;

public interface IImporter
{
    List<string> GetImplementationsNames(List<string> assemblyPaths);
    
    bool Import(string dllPath, string filePath, string type); 
}