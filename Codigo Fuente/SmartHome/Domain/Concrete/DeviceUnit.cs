
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Abstract;

namespace Domain.Concrete;

public class DeviceUnit
{
    [Key] 
    public Guid HardwareId { get; set; }
    public Device Device { get; set; }
  //  public long? RoomId { get; set; }
    public bool IsConnected { get; set; }

    [ForeignKey("RoomId")]
    public Room Room
    {
        get => _room;
        set
        {
            _room = value;
           // RoomId = value.Id;
        }
    }
    
    private Room? _room { get; set; }
    
    public DeviceUnit()
    {
        IsConnected = false;
    }

    public override bool Equals(object? obj)
    {
        return obj is DeviceUnit unit &&
               Device.Id == unit.Device.Id;
    }
}