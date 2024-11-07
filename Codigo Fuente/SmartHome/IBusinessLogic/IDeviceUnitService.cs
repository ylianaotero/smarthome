using Domain.Concrete;

namespace IBusinessLogic;

public interface IDeviceUnitService
{
    void UpdateDeviceCustomName(long id, DeviceUnit updatedDeviceUnit, Guid deviceId);
    void UpdateDeviceConnectionStatus(long homeId, DeviceUnit updatedDeviceUnit);
    void UpdateDeviceRoom(long homeId, DeviceUnit updatedDeviceUnit, Room room);
}