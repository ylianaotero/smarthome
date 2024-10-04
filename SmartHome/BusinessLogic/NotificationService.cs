using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _notificationRepository;
    private readonly IRepository<Home> _homeRepository;
    private const string ElementDoesNotExistExceptionMessage = "Element not found";
    private const string ElementAlreadyExistsExceptionMessage = "Element already exists";
    
    public NotificationService(IRepository<Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public List<Notification> GetNotificationsByFilter(Func<Notification, bool> filter, PageData pageData)
    {
        return _notificationRepository.GetByFilter(filter, null);
    }
    
    public Notification GetNotificationById(int id)
    {
        Notification notification = _notificationRepository.GetById(id);
        if(notification == null)
        {
            throw new ElementNotFound(ElementDoesNotExistExceptionMessage);
        }
        return notification;
    }
    
    private Notification GetBy(Func<Notification, bool> predicate, PageData pageData)
    {
        List<Notification> listOfNotifications = _notificationRepository.GetByFilter(predicate, pageData); 
        Notification notification = listOfNotifications.FirstOrDefault();
        
        if (notification == null)
        {
            throw new ElementNotFound(ElementDoesNotExistExceptionMessage);
        }
        

        return notification; 
    }
    
    public void CreateNotification(Notification notification)
    {
        try
        {
            GetBy(n => n.Event == notification.Event, PageData.Default);
            
            throw new ElementAlreadyExist(ElementAlreadyExistsExceptionMessage);
        }
        catch (ElementNotFound)
        {
            _notificationRepository.Add(notification);
        }
        
    }

    public void SendNotifications(NotificationDTO notificationData)
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

        DeviceUnit device = devices.FirstOrDefault(d => d.HardwareId.ToString() == notificationData.HardwareId.ToString());

        List<Member> membersToSendNotification = home.Members.Where(m => m.ReceivesNotifications).ToList();

        foreach (Member member in membersToSendNotification)
        {
            Notification notification = new Notification();
            notification.Event = notificationData.Event;
            notification.Home = home;
            notification.DeviceUnit = device;
            notification.User = member.User;
            member.Notifications.Add(notification);
        }
    }

}