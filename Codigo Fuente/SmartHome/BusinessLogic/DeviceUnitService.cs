using CustomExceptions;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class DeviceUnitService : IDeviceUnitService
{
    private const string DeviceNotFoundMessage = "Device not found";
    private const string HomeNotFoundMessage = "Home not found";
    private const string RoomNotFoundMessage = "Room not found";
    
    private readonly IRepository<DeviceUnit> _deviceUnitRepository;
    private readonly IHomeService _homeService;
    
    public DeviceUnitService(IRepository<DeviceUnit> deviceUnitRepository, IHomeService homeService)
    {
        this._deviceUnitRepository = deviceUnitRepository;
        this._homeService = homeService;
    }

    public void UpdateDeviceCustomName(long id, DeviceUnit updatedDeviceUnit, Guid deviceId)
    {
        DeviceUnit? deviceUnit = GetDeviceUnitFromHome(id, deviceId);
            
        if (deviceUnit == null)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
            
        deviceUnit.Name = updatedDeviceUnit.Name;
            
        _deviceUnitRepository.Update(deviceUnit);
        _homeService.UpdateHome(id);
    }
    
    public void UpdateDeviceConnectionStatus(long homeId, DeviceUnit updatedDeviceUnit)
    {
        DeviceUnit? deviceUnit = GetDeviceUnitFromHome(homeId, updatedDeviceUnit.HardwareId);
        
        if (deviceUnit == null)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
            
        deviceUnit.IsConnected = updatedDeviceUnit.IsConnected;
            
        _deviceUnitRepository.Update(updatedDeviceUnit);
        _homeService.UpdateHome(homeId);
    }
    
    public void UpdateDeviceRoom(long homeId, DeviceUnit updatedDeviceUnit, Room room)
    {
        DeviceUnit? deviceUnit = GetDeviceUnitFromHome(homeId, updatedDeviceUnit.HardwareId);
        
        if (deviceUnit == null)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
        
        VerifyRoomExistInHome(homeId, room);
            
        deviceUnit.Room = updatedDeviceUnit.Room;
            
        _deviceUnitRepository.Update(updatedDeviceUnit);
        _homeService.UpdateHome(homeId);
    }
    
    private DeviceUnit? GetDeviceUnitFromHome(long homeId, Guid deviceHwId)
    {
        try
        {
            DeviceUnit? device = _homeService
                .GetDevicesFromHome(homeId).Find(d => d.HardwareId == deviceHwId);
            
            return device;
        } 
        catch (Exception)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }
    }
    
    private void VerifyRoomExistInHome(long homeId, Room room)
    {
        bool roomExists = _homeService.GetRoomsFromHome(homeId).Exists(r => r.Id == room.Id);
        
        if (!roomExists)
        {
            throw new CannotFindItemInList(RoomNotFoundMessage);
        }   
    }
}