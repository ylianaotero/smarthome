
using System.ComponentModel.DataAnnotations;
using Domain.Abstract;

namespace Domain.Concrete;

public class DeviceUnit
{
    [Key] 
    public Guid HardwareId { get; set; }
    public Device Device { get; set; }
    public Room Room { get; set; }
    public bool IsConnected { get; set; }
    
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