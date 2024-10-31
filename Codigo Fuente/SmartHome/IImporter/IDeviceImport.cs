using Model.In;

namespace IImporter;

public interface IDeviceImport
{ 
    List<DeviceImportModel> CreateObjectModel(string path);
}