using Domain.Concrete;
using Domain.DTO;

namespace IBusinessLogic;

public interface IDeviceUnitService
{
    void AddDevicesToHome(long homeId, List<DeviceUnitDTO> homeDevices);
    void UpdateDeviceCustomName(long id, DeviceUnit updatedDeviceUnit, Guid deviceId);
    void UpdateDeviceConnectionStatus(long homeId, DeviceUnit updatedDeviceUnit);
    void UpdateDeviceRoom(long homeId, Guid deviceId, long roomId);
}