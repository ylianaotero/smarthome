using CustomExceptions;
using IImporter;
using Model.In;
using Newtonsoft.Json;

namespace JsonDeviceImporter;

public class JsonDevicesImporter : IDeviceImport
{
    private const string DataNotFoundMessage = "Data not found";
    private const string FileNotFoundMessage = "File not found";

    public List<DeviceImportModel> CreateObjectModel(string path)
    {
        try
        {
            var json = File.ReadAllText(path);
            var deviceImportList = JsonConvert.DeserializeObject<DeviceImportList>(json);

            if (deviceImportList != null)
            {
                return deviceImportList.Dispositivos;
            }

            throw new ElementNotFound(DataNotFoundMessage);
        }
        catch (FileNotFoundException)
        {
            throw new ElementNotFound(FileNotFoundMessage);
        }
    }
}