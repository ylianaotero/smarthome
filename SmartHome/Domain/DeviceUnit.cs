
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class DeviceUnit
{
    [Key] 
    public long Id { get; set; }
    public Guid HardwareId { get; set; }
    public Device Device { get; set; }
    public bool IsConnected { get; set; }
    
    public DeviceUnit()
    {
        IsConnected = false;
    }
}