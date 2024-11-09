using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using Domain.DTO;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class DeviceUnitService : IDeviceUnitService
{
    private const string DeviceNotFoundMessage = "Device not found";
    private const string HomeNotFoundMessage = "Home not found";
    private const string RoomNotFoundMessage = "Room not found";
    
    private readonly IRepository<DeviceUnit> _deviceUnitRepository;
    private readonly IDeviceService _deviceService;
    private readonly IHomeService _homeService;
    
    public DeviceUnitService(IRepository<DeviceUnit> deviceUnitRepository, 
        IDeviceService deviceService, IHomeService homeService)
    {
        this._deviceUnitRepository = deviceUnitRepository;
        this._homeService = homeService;
        this._deviceService = deviceService;
    }
  
    public void AddDevicesToHome(long homeId, List<DeviceUnitDTO> homeDevices)
    {
        Home home = _homeService.GetHomeById(homeId);
        
        List<DeviceUnit> devices = new List<DeviceUnit>();
      
        MapDevices(homeDevices, devices);
        
        foreach(DeviceUnit device in devices)
        {
            home.Devices.Add(device);
        }
        
        _homeService.UpdateHome(homeId);
    }
    
    public void UpdateDeviceUnit(long homeId, DeviceUnitDTO deviceUnitDto)
    {
        DeviceUnit deviceUnit = GetDeviceUnitFromHome(homeId, deviceUnitDto.HardwareId) 
                                ?? throw new ElementNotFound(DeviceNotFoundMessage);

        UpdateDeviceUnitProperties(homeId, deviceUnit, deviceUnitDto);

        _deviceUnitRepository.Update(deviceUnit);
        _homeService.UpdateHome(homeId);
    }

    private void UpdateDeviceUnitProperties(long homeId, DeviceUnit deviceUnit, DeviceUnitDTO deviceUnitDto)
    {
        if (deviceUnitDto.IsConnected != null)
        {
            deviceUnit.IsConnected = (bool)deviceUnitDto.IsConnected;
        }

        if (deviceUnitDto.RoomId != null)
        {
            Room room = GetRoomExistFromHome(homeId, (long)deviceUnitDto.RoomId);
            deviceUnit.Room = room;
        }

        if (deviceUnitDto.Name != null)
        {
            deviceUnit.Name = deviceUnitDto.Name;
        }
    }

    private void MapDevices(List<DeviceUnitDTO> homeDevices, List<DeviceUnit> devices)
    {
        foreach (var device in homeDevices)
        {
            if (device.DeviceId == null)
            {
                throw new ElementNotFound(DeviceNotFoundMessage);
            }
            
            Device deviceEntity = _deviceService.GetDeviceById((long)device.DeviceId);

            bool isConnected = false;
            if (!device.IsConnected == null)
            {
                isConnected = (bool)device.IsConnected;
            }

            DeviceUnit deviceUnitService = new DeviceUnit()
            {
                Device = deviceEntity,
                Name = deviceEntity.Name,
                IsConnected = isConnected,
                HardwareId = Guid.NewGuid(),
            };
            
            devices.Add(deviceUnitService);
            
            _deviceUnitRepository.Add(deviceUnitService);
            _deviceUnitRepository.Update(deviceUnitService);
        }
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