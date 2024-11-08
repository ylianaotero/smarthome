using Domain.Concrete;

namespace IBusinessLogic;

public interface IDeviceUnitService
{
    void UpdateDeviceCustomName(long id, DeviceUnitService updatedDeviceUnitService, Guid deviceId);
    void UpdateDeviceConnectionStatus(long homeId, DeviceUnitService updatedDeviceUnitService);
    void UpdateDeviceRoom(long homeId, Guid deviceId, long roomId);
}