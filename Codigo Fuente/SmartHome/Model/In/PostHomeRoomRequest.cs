using Domain.Concrete;

namespace Model.In;

public class PostHomeRoomRequest
{
    public string Name { get; set; }
    
    public Room ToEntity()
    {
        return new Room
        {
            Name = Name
        };
    }
}