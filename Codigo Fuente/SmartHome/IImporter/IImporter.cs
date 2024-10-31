using Domain.Concrete;
using Model.Out;

namespace IImporter;

public interface IImporter
{
    List<ImportResponse> GetImplementationsNamesAndPath(string directoryOfDll);
    
    bool Import(string dllPath, string filePath, string type, List<CompanyOwner> listRoles); 
}