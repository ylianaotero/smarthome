using Domain.DTO;
using IBusinessLogic;

namespace BusinessLogic;

public class ActionService : IActionService
{
    private readonly INotificationService _notificationService;
    private readonly IDeviceUnitService _deviceUnitService;
    
    public ActionService(INotificationService notificationService, IDeviceUnitService deviceUnitService)
    {
        this._notificationService = notificationService;
        this._deviceUnitService = deviceUnitService;
    }
    
    public string PostAction(long homeId, Guid hardwareId, string functionality)
    {
        string message = "The functionality " + functionality + " has been executed in device " +
                                hardwareId + " from home " + homeId + ".";
        
        NotificationDTO notificationData = new NotificationDTO
        {
            Event = message,
            HomeId = homeId,
            HardwareId = hardwareId
        };
        
        _deviceUnitService.ExecuteFunctionality(hardwareId, functionality);
        
        _notificationService.SendNotifications(notificationData);
        
        return message;
    }
}