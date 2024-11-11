
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Abstract;

namespace Domain.Concrete;

public class DeviceUnit
{
    [Key] 
    public Guid HardwareId { get; set; }
    public string Name { get; set; }
    public Device Device { get; set; }
    public bool IsConnected { get; set; }
    public string? Status { get; set; }

    [ForeignKey("RoomId")]
    public Room? Room
    {
        get => _room;
        set
        {
            _room = value;
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
                HardwareId == unit.HardwareId;
    }
}