using Domain.Concrete;

namespace Model.Out;

public class GetRoomsResponse
{
    public List<string> Rooms { get; set; }
    
    public GetRoomsResponse(List<Room> rooms)
    {
        Rooms = rooms.Select(room => room.Name).ToList();
    }

    public override bool Equals(object? obj)
    {
        return obj is GetRoomsResponse response &&
               Enumerable.SequenceEqual(Rooms, response.Rooms);
    }
}