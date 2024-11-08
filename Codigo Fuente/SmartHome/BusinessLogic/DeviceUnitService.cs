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
    
    private readonly IRepository<Domain.Concrete.DeviceUnitService> _deviceUnitRepository;
    private readonly IHomeService _homeService;
    
    public DeviceUnitService(IRepository<Domain.Concrete.DeviceUnitService> deviceUnitRepository, IHomeService homeService)
    {
        this._deviceUnitRepository = deviceUnitRepository;
        this._homeService = homeService;
    }

    public void UpdateDeviceCustomName(long id, Domain.Concrete.DeviceUnitService updatedDeviceUnitService, Guid deviceId)
    {
        Domain.Concrete.DeviceUnitService? deviceUnit = GetDeviceUnitFromHome(id, deviceId);
            
        if (deviceUnit == null)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
            
        deviceUnit.Name = updatedDeviceUnitService.Name;
            
        _deviceUnitRepository.Update(deviceUnit);
        _homeService.UpdateHome(id);
    }
    
    public void UpdateDeviceConnectionStatus(long homeId, Domain.Concrete.DeviceUnitService updatedDeviceUnitService)
    {
        Domain.Concrete.DeviceUnitService? deviceUnit = GetDeviceUnitFromHome(homeId, updatedDeviceUnitService.HardwareId);
        
        if (deviceUnit == null)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
            
        deviceUnit.IsConnected = updatedDeviceUnitService.IsConnected;
            
        _deviceUnitRepository.Update(updatedDeviceUnitService);
        _homeService.UpdateHome(homeId);
    }
    
    public void UpdateDeviceRoom(long homeId, Guid deviceId, long roomId)
    {
        Domain.Concrete.DeviceUnitService? deviceUnit = GetDeviceUnitFromHome(homeId, deviceId);
        
        if (deviceUnit == null)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
        
        Room room = GetRoomExistFromHome(homeId, roomId);

        deviceUnit.Room = room;
            
        _deviceUnitRepository.Update(deviceUnit);
        _homeService.UpdateHome(homeId);
    }
    
    private Domain.Concrete.DeviceUnitService? GetDeviceUnitFromHome(long homeId, Guid deviceHwId)
    {
        try
        {
            Domain.Concrete.DeviceUnitService? device = _homeService
                .GetDevicesFromHome(homeId).Find(d => d.HardwareId == deviceHwId);
            
            return device;
        } 
        catch (Exception)
        {
            throw new ElementNotFound(HomeNotFoundMessage);
        }
    }
    
    private Room GetRoomExistFromHome(long homeId, long roomId)
    {
        Room? room = _homeService.GetRoomsFromHome(homeId).Find(r => r.Id == roomId);
        
        if (room == null)
        {
            throw new CannotFindItemInList(RoomNotFoundMessage);
        }

        return room;
    }
}