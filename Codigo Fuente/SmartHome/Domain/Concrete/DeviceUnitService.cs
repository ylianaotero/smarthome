
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Abstract;

namespace Domain.Concrete;

public class DeviceUnitService
{
    [Key] 
    public Guid HardwareId { get; set; }
    
    public string Name { get; set; }
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
    
    public DeviceUnitService()
    {
        IsConnected = false;
    }

    public override bool Equals(object? obj)
    {
        return obj is DeviceUnitService unit &&
                HardwareId == unit.HardwareId;
    }
}