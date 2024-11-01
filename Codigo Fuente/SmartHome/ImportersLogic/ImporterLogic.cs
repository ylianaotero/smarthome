using System.Reflection;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IImporter;
using Model.In;
using Model.Out;

namespace ImportersLogic;

public class ImporterLogic : IImporter.IImporter
{
    private IDeviceService _deviceService; 
    
    public ImporterLogic(IDeviceService deviceService)
    {
        _deviceService = deviceService; 
    }

    public List<ImportResponse> GetImplementationsNamesAndPath()
    {
        string directoryOfDll = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\DLLsImports"));

        var assemblyPaths = Directory.GetFiles(directoryOfDll, "*.dll", SearchOption.AllDirectories);
        var assemblies = assemblyPaths
            .Select(Assembly.LoadFrom)
            .ToList();

        var implementations = FindImplementations(assemblies);

        var ret = new List<ImportResponse>();

        foreach (var (name, location) in implementations)
        {
            ImportResponse importResponse = new ImportResponse(); 
            Console.WriteLine($"Implementación: {name}, Ubicación: {location}");
            importResponse.AssemblyLocation = location;
            importResponse.ImplementationName = name; 
            ret.Add(importResponse);
        }

        return ret;

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
        if (type.ToLower().Equals("json") && File.Exists(path))
        {
            Assembly jsonAssembly = Assembly.LoadFrom(path);
            foreach (var item in jsonAssembly.GetTypes().Where(t => typeof(IDeviceImport).IsAssignableFrom(t)))
            {
                if (item.FullName.ToLower().Contains("json"))
                {
                    return Activator.CreateInstance(item) as IDeviceImport;
                }
            }
        }
        throw new ElementNotFound("Type or dll not found");
    }
}