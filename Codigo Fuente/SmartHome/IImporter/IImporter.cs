using Domain.Concrete;
using Model.Out;

namespace IImporter;

public interface IImporter
{
    List<ImportResponse> GetImplementationsNamesAndPath();
    
    bool Import(string dllPath, string filePath, string type, List<Company> listRoles); 
    
    bool MoveDllFile(string sourcePath);
}