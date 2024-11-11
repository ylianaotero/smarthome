using IBusinessLogic;

namespace BusinessLogic;

public class ActionService
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
        string expectedResult = "The functionality " + functionality + " has been executed in device " +
                                hardwareId + " from home " + homeId + ". Status has been changed to On";
        return expectedResult;
    }
}