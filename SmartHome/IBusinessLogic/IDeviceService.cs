using Domain.Abstract;
using IDataAccess;

namespace IBusinessLogic;

public interface IDeviceService
{
    void CreateDevice(Device device);
    Device GetDeviceById(long id);
    List<Device> GetDevicesByFilter(Func<Device, bool> filter, PageData pageData);
    List<string> GetDeviceTypes();
}