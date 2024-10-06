using Domain.Concrete;

namespace Model.In;

public class GetNotificationsRequest
{
    public int? HomeId { get; set; }
    public int? UserId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool? Read { get; set; }
    public string? Kind { get; set; }
    
    public Func<Notification,bool> ToFilter()
    {
        return notification => (HomeId == 0 || notification.Home.Id == HomeId) &&
                               (UserId == 0 || notification.Member.User.Id == UserId) &&
                               (CreatedAt == DateTime.MinValue || notification.CreatedAt == CreatedAt) &&
                               (Read == false || notification.Read == Read) &&
                               (string.IsNullOrEmpty(Kind) || notification.DeviceUnit.Device.Kind.ToLower() == Kind.ToLower());
    }
}