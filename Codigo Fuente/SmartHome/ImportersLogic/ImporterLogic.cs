using System.Reflection;
using CustomExceptions;
using Domain.Concrete;
using IBusinessLogic;
using IImporter;
using Model.In;
using Model.Out;

namespace ImportersLogic;

public class ImporterLogic : IImporter.IImporter
{
    private const string DllSearchPattern = "*.dll";
    private const string DllDirectoryPath = @"DLLsImports";
    private const string JsonImportType = "json";
    private const string JsonImplementationName = "json";
    private const string FileNotFoundMessage = "File not found";
    private const string TypeOrDllNotFoundMessage = "Type or dll not found";

    private IDeviceService _deviceService; 
    
    public ImporterLogic(IDeviceService deviceService)
    {
        _deviceService = deviceService; 
    }

    public List<ImportResponse> GetImplementationsNamesAndPath()
    
    
    {
        string baseDirectory = AppContext.BaseDirectory;
        
        string codigoFuenteDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName ?? throw new InvalidOperationException("No se pudo encontrar el directorio 'Codigo Fuente'.");
        
        string directoryOfDlll = Path.Combine(codigoFuenteDirectory, DllDirectoryPath);
        
        string directoryOfDll = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), directoryOfDlll));
        

        var assemblyPaths = Directory.GetFiles(directoryOfDll, DllSearchPattern, SearchOption.AllDirectories);
        var assemblies = assemblyPaths
            .Select(Assembly.LoadFrom)
            .ToList();

        var implementations = FindImplementations(assemblies);

        var ret = new List<ImportResponse>();

        foreach (var (name, location) in implementations)
        {
            ImportResponse importResponse = new ImportResponse(); 
            importResponse.AssemblyLocation = location;
            importResponse.ImplementationName = name; 
            ret.Add(importResponse);
        }

        return ret;
    }
    
    public bool MoveDllFile(string sourcePath)
    {
        try
        {
            if (Path.GetExtension(sourcePath).ToLower() != ".dll")
            {
                return false;  
            }
            
            string directoryOfDll = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), DllDirectoryPath));
            
            if (File.Exists(sourcePath))
            {
                string fileName = Path.GetFileName(sourcePath);
                
                string destinationPath = Path.Combine(directoryOfDll, fileName);
                
                File.Move(sourcePath, destinationPath);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    private List<(string ImplementationName, string AssemblyLocation)> FindImplementations(List<Assembly> assemblies)
    {
        var ret = new List<(string ImplementationName, string AssemblyLocation)>();

        foreach (var assembly in assemblies)
        {
            var implementations = assembly
                .GetTypes()
                .Where(t => typeof(IDeviceImport).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var imp in implementations)
            {
                ret.Add((imp.FullName, assembly.Location));
            }
        }

        return ret;
    }

    public bool Import(string dllPath, string filePath, string type, List<Company> listOfCompanies)
    {
        IDeviceImport requestedImplementation = GetImplementation(dllPath, type);
        List<DeviceImportModel> imported = requestedImplementation.CreateObjectModel(filePath);
        foreach (DeviceImportModel device in imported)
        {
            foreach (var company in listOfCompanies)
            {
                _deviceService.CreateDevice(device.ToEntity(company));
            }
        }
        return true;
    }

    private IDeviceImport GetImplementation(string path, string type)
    {
        try
        {
            if (type.ToLower().Equals(JsonImportType) && File.Exists(path))
            {
                Assembly jsonAssembly = Assembly.LoadFrom(path);
                foreach (var item in jsonAssembly.GetTypes().Where(t => typeof(IDeviceImport).IsAssignableFrom(t)))
                {
                    if (item.FullName != null && item.FullName.ToLower().Contains(JsonImplementationName))
                    {
                        IDeviceImport deviceImport = Activator.CreateInstance(item) as IDeviceImport;
                        if (deviceImport != null)
                        {
                            return deviceImport ;
                        }
                        
                    }
                }
            }
            throw new ElementNotFound(TypeOrDllNotFoundMessage);
        }
        catch (FileNotFoundException ex)
        {
            throw new ElementNotFound(FileNotFoundMessage);
        }
    }
}
