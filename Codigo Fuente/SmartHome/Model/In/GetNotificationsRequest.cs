using Domain.Concrete;

namespace Model.In;

public class GetNotificationsRequest
{
    public int? HomeId { get; set; }
    public int? UserId { get; set; }
    public bool? Read { get; set; }
    public string? Kind { get; set; }
    
    public Func<Notification,bool> ToFilter()
    {
        return notification => (HomeId == null || notification.Home?.Id == HomeId) &&
                               (UserId == null || notification.Member?.User?.Id == UserId) &&
                               (Read == null || notification.Read == Read) &&
                               (string.IsNullOrEmpty(Kind) 
                                || notification.DeviceUnitService?.Device?.Kind?.ToLower() == Kind.ToLower());
    }
}