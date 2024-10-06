using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class NotificationService(IRepository<Notification> notificationRepository) : INotificationService
{
    private readonly IRepository<Home> _homeRepository;
    private const string ElementDoesNotExistExceptionMessage = "Element not found";

    public List<Notification> GetNotificationsByFilter(Func<Notification, bool> filter, PageData pageData)
    {
        return notificationRepository.GetByFilter(filter, null);
    }
    
    public Notification GetNotificationById(int id)
    {
        Notification notification = notificationRepository.GetById(id);
        if(notification == null)
        {
            throw new ElementNotFound(ElementDoesNotExistExceptionMessage);
        }
        return notification;
    }

    public void SendNotifications(NotificationDTO notificationData)
    {
        (Home home, DeviceUnit device) = ExtractNotificationDTOObjects(notificationData);
        
        List<Member> membersToSendNotification = home.Members.Where(m => m.ReceivesNotifications).ToList();

        foreach (Member member in membersToSendNotification)
        {
            Notification notification = new Notification();
            
            notification.Event = notificationData.Event;
            notification.Home = home;
            notification.DeviceUnit = device;
            notification.Member = member;
            
            member.Notifications.Add(notification);
        }
    }
    
    private (Home, DeviceUnit) ExtractNotificationDTOObjects(NotificationDTO notificationData)
    {
        Home home = _homeRepository.GetById(notificationData.HomeId);
        if(home == null)
        {
            throw new ElementNotFound(ElementDoesNotExistExceptionMessage);
        }
        
        List<DeviceUnit> devices = home.Devices;
        if(devices == null)
        {
            throw new ElementNotFound(ElementDoesNotExistExceptionMessage);
        }

        DeviceUnit device = devices
            .FirstOrDefault(d => d.HardwareId.ToString() == notificationData.HardwareId.ToString());

        if(device == null)
        {
            throw new ElementNotFound(ElementDoesNotExistExceptionMessage);
        }
        
        return (home, device);
    }
}