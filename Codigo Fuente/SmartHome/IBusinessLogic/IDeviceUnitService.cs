using Domain.DTO;

namespace IBusinessLogic;

public interface IDeviceUnitService
{
    void AddDevicesToHome(long homeId, List<DeviceUnitDTO> homeDevices);
    void UpdateDeviceUnit(long homeId, DeviceUnitDTO deviceUnitDto);
    void ExecuteFunctionality(Guid hardwareId, string functionality);
}