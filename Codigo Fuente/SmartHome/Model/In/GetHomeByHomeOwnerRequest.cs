using Domain.Concrete;

namespace Model.In;

public class GetHomeByHomeOwnerRequest
{
    public long? HomeOwnerId { get; set; }

    public Func<Home, bool> ToFilter()
    {
        return home => (!HomeOwnerId.HasValue || home.Owner.Id == HomeOwnerId);
    }
}