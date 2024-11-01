using CustomExceptions;
using IImporter;
using Model.In;
using Newtonsoft.Json;

namespace JsonDeviceImporter;

public class JsonDevicesImporter : IDeviceImport
{
    public List<DeviceImportModel> CreateObjectModel(string path)
    {
        try
        {
            var json = File.ReadAllText(path);
            var deviceImportList = JsonConvert.DeserializeObject<DeviceImportList>(json);
            return deviceImportList.Dispositivos;
        }
        catch (FileNotFoundException)
        {
            throw new ElementNotFound("Archivo no encontrado: " + path);
        }
    }
}