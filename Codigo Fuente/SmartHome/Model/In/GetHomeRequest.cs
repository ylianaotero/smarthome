using Domain.Concrete;

namespace Model.In;

public class GetHomeRequest
{
    public string? Street { get; set; }
    public string? DoorNumber { get; set; }
    
    public long? HomeOwnerId { get; set; }

    public Func<Home, bool> ToFilter()
    {
        return home => (string.IsNullOrEmpty(Street) || home.Street == Street) &&
                       (string.IsNullOrEmpty(DoorNumber) || home.DoorNumber == int.Parse(DoorNumber)) &&
                       (!HomeOwnerId.HasValue || home.Owner.Id == HomeOwnerId);
    }
}