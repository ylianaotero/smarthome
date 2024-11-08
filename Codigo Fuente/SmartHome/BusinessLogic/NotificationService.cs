using CustomExceptions;
using Domain.Concrete;
using Domain.DTO;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class NotificationService
    (IRepository<Notification> notificationRepository, IRepository<Home> homeRepository) : INotificationService
{
    private const string ElementDoesNotExistExceptionMessage = "Element not found";

    public List<Notification> GetNotificationsByFilter(Func<Notification, bool> filter, PageData pageData)
    {
        return notificationRepository.GetByFilter(filter, null);
    }
  
    public void SendNotifications(NotificationDTO notificationData)
    {
        (Home home, Domain.Concrete.DeviceUnitService device) = ExtractNotificationDTOObjects(notificationData);

        if (device.IsConnected)
        {
            List<Member> membersToSendNotification = home.Members.Where(m => m.ReceivesNotifications).ToList();

            foreach (Member member in membersToSendNotification)
            {
                Notification notification = new Notification();
            
                notification.Event = notificationData.Event;
                notification.Home = home;
                notification.DeviceUnitService = device;
                notification.Member = member;
                notification.ReadAt = DateTime.MinValue; 
                
                member.Notifications.Add(notification);
                notificationRepository.Add(notification);
            }
        }
    }
    
    private (Home, Domain.Concrete.DeviceUnitService) ExtractNotificationDTOObjects(NotificationDTO notificationData)
    {
        Home home = homeRepository.GetById(notificationData.HomeId);
        if(home == null)
        {
            throw new ElementNotFound(ElementDoesNotExistExceptionMessage);
        }
        
        List<Domain.Concrete.DeviceUnitService> devices = home.Devices;
        if (devices == null)
        {
            throw new ElementNotFound(ElementDoesNotExistExceptionMessage);
        }

        Domain.Concrete.DeviceUnitService device = devices
            .FirstOrDefault(d => d.HardwareId.ToString() == notificationData.HardwareId.ToString());

        if (device == null)
        {
            throw new ElementNotFound(ElementDoesNotExistExceptionMessage);
        }
        
        return (home, device);
    }
}